using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;
using System.Security.Cryptography;

namespace OVB.Demos.FakeBank.UnitTests.ValueObjects;

public sealed class OffuscationTokenValueObjectValidationTests
{
    [Fact]
    public void Should_Be_Offuscation_Token_Created_With_Successfull()
    {
        // Arrange

        // Act
        var token = OffuscationTokenValueObject.Build();

        // Assert
        Assert.True(token.IsValid);
        Assert.Empty(token.GetMethodResult().Notifications);
        Assert.True(token.GetMethodResult().IsSuccess);
    }

    [Fact]
    public void Should_Be_Offuscation_Token_Equal_A_Expected_Sent()
    {
        // Arrange
        using var aes = Aes.Create();
        aes.GenerateIV();
        var key = aes.IV;
        var keyBase64 = Convert.ToBase64String(key);

        // Act
        var offuscationToken = OffuscationTokenValueObject.Build(
            token: key);

        // Assert
        Assert.Equal(
            expected: key,
            actual: offuscationToken.GetOffuscationToken());
        Assert.Equal(
            expected: keyBase64,
            actual: offuscationToken);
    }

    [Fact]
    public void Should_Notification_As_Expected_When_The_IV_Key_Is_Not_Valid()
    {
        // Arrange
        var key = new byte[12];
        var index = 0;

        const string codeMessageNotificationExpected = "OFFUSCATION_TOKEN_VALUE_OBJECT";
        const string messageNotificationExpected = "A chave secreta de ofuscação da aplicação cliente precisa ser válida.";

        // Act
        var offuscationToken = OffuscationTokenValueObject.Build(
            token: key,
            index: index);

        // Assert
        Assert.False(offuscationToken.IsValid);
        Assert.Single(offuscationToken.GetMethodResult().Notifications);
        Assert.Equal(
            expected: codeMessageNotificationExpected,
            actual: offuscationToken.GetMethodResult().Notifications[0].Code);
        Assert.Equal(
            expected: messageNotificationExpected,
            actual: offuscationToken.GetMethodResult().Notifications[0].Message);
        Assert.Equal(
            expected: index,
            actual: offuscationToken.GetMethodResult().Notifications[0].Index);
    }
}
