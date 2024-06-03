using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects.Exceptions;
using OVB.Demos.FakeBank.MethodResultContext;
using OVB.Demos.FakeBank.NotificationContext;
using OVB.Demos.FakeBank.NotificationContext.Interfaces;
using System.Security.Cryptography;

namespace OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

/// <summary>
/// Chave Secreta de Autenticação de uma Conta Corrente
/// 
/// Essa chave secreta de autenticação, também é utilizada como chave de ofuscação de dados. Os dados só são abertos, com a descriptografia adequada.
/// </summary>
public readonly struct OffuscationTokenValueObject
{
    public bool IsValid { get; }
    private byte[] OffuscationToken { get; }
    private MethodResult<INotification> MethodResult { get; }

    private OffuscationTokenValueObject(bool isValid, byte[] offuscationToken, MethodResult<INotification> methodResult)
    {
        IsValid = isValid;
        OffuscationToken = offuscationToken;
        MethodResult = methodResult;
    }

    private static INotification OffuscationTokenValueObjectMustBeValidAsExpected(int? index = null) => Notification.BuildError(
        code: "OFFUSCATION_TOKEN_VALUE_OBJECT",
        message: "A chave secreta de ofuscação da aplicação cliente precisa ser válida.",
        index: index);

    public static OffuscationTokenValueObject Build(byte[]? token = null, int? index = null)
    {
        const int tokenIvLengthRequired = 16;

        if (token is null)
            return new OffuscationTokenValueObject(
                isValid: true,
                offuscationToken: GenerateOffuscationToken(),
                methodResult: MethodResult<INotification>.BuildSuccessResult());

        if (token.Length != tokenIvLengthRequired)
            return new OffuscationTokenValueObject(
                isValid: false,
                offuscationToken: [],
                methodResult: MethodResult<INotification>.BuildFailureResult(
                    notifications: [OffuscationTokenValueObjectMustBeValidAsExpected(index)]));

        return new OffuscationTokenValueObject(
            isValid: true,
            offuscationToken: token,
            methodResult: MethodResult<INotification>.BuildSuccessResult());
    }

    private static byte[] GenerateOffuscationToken()
    {
        using var aes = Aes.Create();
        aes.GenerateIV();
        return aes.IV;
    }

    public string GetOffuscationTokenToBase64String()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);
        
        return Convert.ToBase64String(OffuscationToken);
    }

    public byte[] GetOffuscationToken()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return OffuscationToken;
    }

    public static implicit operator OffuscationTokenValueObject(byte[] obj)
        => Build(obj);
    public static implicit operator MethodResult<INotification>(OffuscationTokenValueObject obj)
        => obj.GetMethodResult();
    public static implicit operator byte[](OffuscationTokenValueObject obj)
        => obj.GetOffuscationToken();
    public static implicit operator string(OffuscationTokenValueObject obj)
        => obj.GetOffuscationTokenToBase64String();

    public MethodResult<INotification> GetMethodResult()
        => MethodResult;
}
