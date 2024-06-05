using OVB.Demos.FakeBank.Account.Domain.AccountContext.Enumerators;
using OVB.Demos.FakeBank.Account.Domain.AccountContext.ValueObjects;

namespace OVB.Demos.FakeBank.UnitTests.ValueObjects;

public sealed class AccountTypeValidationTests
{
    [Theory]
    [InlineData("CURRENT")]
    public void Should_Be_Account_Type_Value_Object_Valid(string accountType)
    {
        // Arrange

        // Act
        var accountTypeValueObject = AccountTypeValueObject.Build(accountType);

        // Assert
        Assert.True(accountTypeValueObject.IsValid);
        Assert.True(accountTypeValueObject.GetMethodResult().IsSuccess);
        Assert.Empty(accountTypeValueObject.GetMethodResult().Notifications);
    }

    [Theory]
    [InlineData("ESCROW")]
    [InlineData("SALARY")]
    [InlineData("PAYMENT")]
    public void Should_Be_Account_Type_Value_Object_Not_Valid(string accountType)
    {
        // Arrange

        // Act
        var accountTypeValueObject = AccountTypeValueObject.Build(accountType);

        // Assert
        Assert.False(accountTypeValueObject.IsValid);
        Assert.False(accountTypeValueObject.GetMethodResult().IsSuccess);
        Assert.Single(accountTypeValueObject.GetMethodResult().Notifications);
    }

    [Theory]
    [InlineData("ESCROW")]
    [InlineData("SALARY")]
    [InlineData("PAYMENT")]
    public void Should_Be_Notification_When_Account_Type_Is_Not_Valid(string accountType)
    {
        // Arrange
        const string codeNotificationExpected = "ACCOUNT_TYPE_VALUE_OBJECT_NOT_DEFINED";
        const string messageNotificationExpected = "O tipo da conta bancária precisa ser um enumerador suportado pela plataforma.";
        const int indexNotificationExpected = 0;

        // Act
        var accountTypeValueObject = AccountTypeValueObject.Build(accountType);

        // Assert
        Assert.Equal(
            expected: codeNotificationExpected,
            actual: accountTypeValueObject.GetMethodResult().Notifications[0].Code);
        Assert.Equal(
            expected: messageNotificationExpected,
            actual: accountTypeValueObject.GetMethodResult().Notifications[0].Message);
        Assert.Equal(
            expected: indexNotificationExpected,
            actual: accountTypeValueObject.GetMethodResult().Notifications[0].Index);
    }

    [Fact]
    public void Should_Be_Get_Method_Info_About_Type_As_Expected()
    {
        // Arrange
        const string expectedType = "CURRENT";

        // Act
        var accountTypeValueObject = AccountTypeValueObject.Build(expectedType);

        // Assert
        Assert.Equal(
            expected: expectedType,
            actual: accountTypeValueObject.GetTypeAccountToString());
        Assert.Equal(
            expected: Enum.Parse<TypeAccount>(expectedType),
            actual: accountTypeValueObject.GetTypeAccount());
    }
}
