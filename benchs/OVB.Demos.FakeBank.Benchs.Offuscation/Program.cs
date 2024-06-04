using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;
using System.Security.Cryptography;
using System.Text.Json;

namespace OVB.Demos.FakeBank.Benchs.Offuscation;

public static class Program
{
    static void Main(string[] args)
    {
        using var aes = Aes.Create();
        aes.GenerateKey();

        var identity = IdentityValueObject.Build();
        var offuscationToken = OffuscationTokenValueObject.Build();
        var idOffuscated = IdentityObfuscatedValueObject.Build(
            identity: identity,
            offuscationToken: offuscationToken,
            privateKey: aes.Key);

        Console.WriteLine(JsonSerializer.Serialize(new
        {
            id = identity.GetIdentityIdAsString(),
            offuscatedId = offuscationToken.GetOffuscationTokenToBase64String(),
            offuscation = idOffuscated.GetIdentityObfuscatedToBase64String(),
            key = Convert.ToBase64String(aes.Key)
        }));
    }
}
