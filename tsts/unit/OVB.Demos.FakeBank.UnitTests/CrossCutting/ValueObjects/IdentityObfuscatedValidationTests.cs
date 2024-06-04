using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

namespace OVB.Demos.FakeBank.UnitTests.CrossCutting.ValueObjects;

public sealed class IdentityObfuscatedValidationTests
{
    [Fact]
    public void Should_Be_Identity_Obfuscated_Is_Equal_The_Expected()
    {
        // Arrange
        byte[] privateKey = Convert.FromBase64String("YOvCjqdO5lKfOF5iVQ3NbauxKzfFJx0UskGXs7S4cfk=");
        byte[] publicKey = Convert.FromBase64String("PTQdoeEnxnDn419e\u002BD4lFA==");
        string ulidExample = "01HZGBM9JQMGBMDNBDSAVD1BWF";
        string ulidOffuscated = "syJ1MTFnx3kw75IMZQ2qHD0MvJ8gYPif0R5NJsr/dPY=";

        var offuscationToken = OffuscationTokenValueObject.Build(
            token: publicKey);
        var identityValueObject = IdentityValueObject.Build(
            identityId: Ulid.Parse(ulidExample));

        // Act
        var identityObfuscated = IdentityObfuscatedValueObject.Build(
            identity: identityValueObject,
            offuscationToken: offuscationToken,
            privateKey: privateKey);

        // Assert
        Assert.True(identityObfuscated.IsValid);
        Assert.Empty(identityObfuscated.GetMethodResult().Notifications);
        Assert.True(identityObfuscated.GetMethodResult().IsSuccess);
        Assert.Equal(
            expected: ulidOffuscated,
            actual: identityObfuscated.GetIdentityObfuscatedToBase64String());
        Assert.Equal(
            expected: Convert.FromBase64String(ulidOffuscated),
            actual: identityObfuscated.GetIdentityObfuscated());
    }

    [Fact]
    public void Should_Be_Identity_Obfuscated_Is_Not_Valid_When_Send_Invalid_Identity()
    {
        // Arrange
        var ulid = Ulid.Empty;
        var identity = IdentityValueObject.Build(
            identityId: ulid);

        // Act
        var identityObfuscated = IdentityObfuscatedValueObject.Build(
            identity: identity,
            offuscationToken: new byte[16],
            privateKey: new byte[16]);

        // Assert
        Assert.False(identityObfuscated.IsValid);
        Assert.Equal(
            expected: identity.GetMethodResult().Notifications,
            actual: identityObfuscated.GetMethodResult().Notifications);
        Assert.False(identityObfuscated.GetMethodResult().IsSuccess);
    }

    [Fact]
    public void Should_Be_Identity_Obfuscated_Is_Not_Valid_When_Send_Invalid_OffuscationToken()
    {
        // Arrange
        var ulid = Ulid.NewUlid();
        var identity = IdentityValueObject.Build(
            identityId: ulid);
        var offuscationToken = OffuscationTokenValueObject.Build(
            token: new byte[14]);

        // Act
        var identityObfuscated = IdentityObfuscatedValueObject.Build(
            identity: identity,
            offuscationToken: offuscationToken,
            privateKey: new byte[16]);

        // Assert
        Assert.False(identityObfuscated.IsValid);
        Assert.Equal(
            expected: offuscationToken.GetMethodResult().Notifications,
            actual: identityObfuscated.GetMethodResult().Notifications);
        Assert.False(identityObfuscated.GetMethodResult().IsSuccess);
    }
}
