using OVB.Demos.FakeBank.Account.Domain.AccountContext.Enumerators;
using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects.Exceptions;
using OVB.Demos.FakeBank.MethodResultContext;
using OVB.Demos.FakeBank.NotificationContext;
using OVB.Demos.FakeBank.NotificationContext.Interfaces;

namespace OVB.Demos.FakeBank.Account.Domain.AccountContext.ValueObjects;

public readonly struct AccountStatusValueObject
{
    public bool IsValid { get; }
    private TypeAccountStatus Status { get; }
    private MethodResult<INotification> MethodResult { get; }

    private AccountStatusValueObject(bool isValid, TypeAccountStatus status, MethodResult<INotification> methodResult)
    {
        IsValid = isValid;
        Status = status;
        MethodResult = methodResult;
    }

    private static INotification AccountStatusIsNotDefined(int? index = null) => Notification.BuildError(
        code: "TYPE_ACCOUNT_STATUS_NOT_DEFINED",
        message: "O tipo do status da conta bancária não é suportado pela plataforma",
        index: index);

    public static AccountStatusValueObject Build(string status, int? index = null)
    {
        var isPossibleToConvert = Enum.TryParse<TypeAccountStatus>(
            value: status,
            ignoreCase: false,
            result: out var typeAccountStatus);

        if (isPossibleToConvert)
            return new AccountStatusValueObject(
                isValid: true,
                status: typeAccountStatus,
                methodResult: MethodResult<INotification>.BuildSuccessResult());
        else return new AccountStatusValueObject(
            isValid: false,
            status: 0,
            methodResult: MethodResult<INotification>.BuildFailureResult(
                notifications: [AccountStatusIsNotDefined(index)]));
    }

    public static AccountStatusValueObject Build(string status)
        => Build(status, null);

    public TypeAccountStatus GetAccountStatus()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return Status;
    }

    public string GetAccountStatusToString()
        => GetAccountStatus().ToString();

    public static implicit operator string(AccountStatusValueObject obj)
        => obj.GetAccountStatusToString();
    public static implicit operator TypeAccountStatus(AccountStatusValueObject obj)
        => obj.GetAccountStatus();
    public static implicit operator MethodResult<INotification>(AccountStatusValueObject obj)
        => obj.GetMethodResult();

    public MethodResult<INotification> GetMethodResult()
        => MethodResult;
}
