using OVB.Demos.FakeBank.Account.Domain.AccountContext.Enumerators;
using OVB.Demos.FakeBank.Account.Domain.AccountContext.ValueObjects;

namespace OVB.Demos.FakeBank.UnitTests.ValueObjects;

public sealed class AccountStatusValueObjectValidationTests
{
    [Theory]
    [InlineData("PENDING_DOCUMENTATION")]
    [InlineData("PENDING_ANALYSIS")]
    [InlineData("ACTIVE")]
    [InlineData("DECLINED")]
    [InlineData("BLOCKED")]
    public void Should_Be_Creation_Of_Account_Status_Value_Object_Is_Valid(string status)
    {
        // Arrange 

        // Act
        var accountStatus = AccountStatusValueObject.Build(
            status: status);

        // Assert
        Assert.True(accountStatus.IsValid);
        Assert.Empty(accountStatus.GetMethodResult().Notifications);
        Assert.True(accountStatus.GetMethodResult().IsSuccess);
    }

    [Theory]
    [InlineData("PENDING")]
    [InlineData("LIVE")]
    [InlineData("EFFECTIVE")]
    [InlineData("DEACTIVE")]
    public void Should_Be_Creation_Of_Account_Status_Value_Object_Not_Valid(string status)
    {
        // Arrange 
        const string codeNotificationExpected = "TYPE_ACCOUNT_STATUS_NOT_DEFINED";
        const string messageNotificationExpected = "O tipo do status da conta bancária não é suportado pela plataforma";
        const int indexNotificationExpected = 0;

        // Act
        var accountStatus = AccountStatusValueObject.Build(
            status: status,
            index: indexNotificationExpected);

        // Assert
        Assert.False(accountStatus.IsValid);
        Assert.Single(accountStatus.GetMethodResult().Notifications);
        Assert.False(accountStatus.GetMethodResult().IsSuccess);
        Assert.Equal(
            expected: codeNotificationExpected,
            actual: accountStatus.GetMethodResult().Notifications[0].Code);
        Assert.Equal(
            expected: messageNotificationExpected,
            actual: accountStatus.GetMethodResult().Notifications[0].Message);
        Assert.Equal(
            expected: indexNotificationExpected,
            actual: accountStatus.GetMethodResult().Notifications[0].Index);
    }
}
