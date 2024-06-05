using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects.Exceptions;
using OVB.Demos.FakeBank.MethodResultContext;
using OVB.Demos.FakeBank.NotificationContext.Interfaces;
using System.Security.Cryptography;

namespace OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

public readonly struct IdentityObfuscatedValueObject
{
    public bool IsValid { get; }
    private byte[] IdentityObfuscated { get; }
    private MethodResult<INotification> MethodResult { get; }

    private IdentityObfuscatedValueObject(bool isValid, byte[] identityObfuscated, MethodResult<INotification> methodResult)
    {
        IsValid = isValid;
        IdentityObfuscated = identityObfuscated;
        MethodResult = methodResult;
    }

    /// <summary>
    /// Criar Objeto de Valor de Identidade Ofuscado - IdentityObsfucatedValueObject
    /// 
    /// <para>
    /// Utilize esse objeto para validar uma chave pública da aplicação cliente (<paramref name="offuscationToken"/>) enviada com base na criptografia do 
    /// ID da Entidade (<paramref name="identity"/>) + Chave Privada da Aplicação servidora (<paramref name="privateKey"/>).
    /// </para>
    /// </summary>
    /// <param name="identity">ID da Identidade</param>
    /// <param name="offuscationToken">Token de Ofuscação criado para a Aplicação Cliente</param>
    /// <param name="privateKey">Chave Privada da Aplicação de Criptografia</param>
    /// <returns>ID da Identidade ofuscado com a Criptografia</returns>
    public static IdentityObfuscatedValueObject Build(
        IdentityValueObject identity,
        OffuscationTokenValueObject offuscationToken,
        byte[] privateKey)
    {
        if (!identity.IsValid || !offuscationToken.IsValid)
            return new IdentityObfuscatedValueObject(
                isValid: false,
                identityObfuscated: [],
                methodResult: MethodResult<INotification>.BuildFromAnothersMethodResults(
                    offuscationToken.GetMethodResult(), identity.GetMethodResult()));

        return new IdentityObfuscatedValueObject(
            isValid: true,
            identityObfuscated: EncryptDataUsingAsymmetricAlgorithm(
                privateKey: privateKey,
                publicKey: offuscationToken.GetOffuscationToken(),
                data: identity.GetIdentityIdAsString()),
            methodResult: MethodResult<INotification>.BuildSuccessResult());
    }

    public static IdentityObfuscatedValueObject Build(string obfuscatedId)
        => new IdentityObfuscatedValueObject(
            isValid: true,
            identityObfuscated: Convert.FromBase64String(obfuscatedId),
            methodResult: MethodResult<INotification>.BuildSuccessResult());

    private static byte[] EncryptDataUsingAsymmetricAlgorithm(
        byte[] privateKey,
        byte[] publicKey,
        string data)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = privateKey;
            aes.IV = publicKey;

            using var memoryStream = new MemoryStream();
            using (CryptoStream cryptoStream = new(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                using (var source = new StreamWriter(cryptoStream))
                    source.Write(data);

                return memoryStream.ToArray();
            }
        }
    }

    public byte[] GetIdentityObfuscated()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return IdentityObfuscated;
    }

    public string GetIdentityObfuscatedToBase64String()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return Convert.ToBase64String(IdentityObfuscated);
    }

    public static implicit operator string(IdentityObfuscatedValueObject obj)
        => obj.GetIdentityObfuscatedToBase64String();
    public static implicit operator byte[](IdentityObfuscatedValueObject obj)
        => obj.GetIdentityObfuscated();
    public static implicit operator MethodResult<INotification>(IdentityObfuscatedValueObject obj)
        => obj.GetMethodResult();

    public MethodResult<INotification> GetMethodResult()
        => MethodResult;
}
