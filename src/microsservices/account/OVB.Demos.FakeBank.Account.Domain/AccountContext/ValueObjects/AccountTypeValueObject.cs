using OVB.Demos.FakeBank.Account.Domain.AccountContext.Enumerators;
using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects.Exceptions;
using OVB.Demos.FakeBank.MethodResultContext;
using OVB.Demos.FakeBank.NotificationContext;
using OVB.Demos.FakeBank.NotificationContext.Interfaces;

namespace OVB.Demos.FakeBank.Account.Domain.AccountContext.ValueObjects;

public readonly struct AccountTypeValueObject
{
    public bool IsValid { get; }
    private TypeAccount TypeAccount { get; }
    private MethodResult<INotification> MethodResult { get; }

    private AccountTypeValueObject(bool isValid, TypeAccount typeAccount, MethodResult<INotification> methodResult)
    {
        IsValid = isValid;
        TypeAccount = typeAccount;
        MethodResult = methodResult;
    }

    private static INotification AccountTypeNotificationNotDefined(int? index = null)
        => Notification.BuildError(
            code: "ACCOUNT_TYPE_VALUE_OBJECT_NOT_DEFINED",
            message: "O tipo da conta bancária precisa ser um enumerador suportado pela plataforma",
            index: index);

    public static AccountTypeValueObject Build(string type, int? index = null)
    {
        var isPossibleToConvertEnumerator = Enum.TryParse<TypeAccount>(
            value: type,
            ignoreCase: false,
            result: out var typeAccount);

        if (!isPossibleToConvertEnumerator)
            return new AccountTypeValueObject(
                isValid: false,
                typeAccount: 0,
                methodResult: MethodResult<INotification>.BuildFailureResult(
                    notifications: [AccountTypeNotificationNotDefined(index)]));
        else return new AccountTypeValueObject(
            isValid: true,
            typeAccount: typeAccount,
            methodResult: MethodResult<INotification>.BuildSuccessResult());
    }

    public TypeAccount GetTypeAccount()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return TypeAccount;
    }

    public string GetTypeAccountToString()
        => GetTypeAccount().ToString();

    public static implicit operator MethodResult<INotification>(AccountTypeValueObject obj)
        => obj.GetMethodResult();
    public static implicit operator string(AccountTypeValueObject obj)
        => obj.GetTypeAccountToString();
    public static implicit operator TypeAccount(AccountTypeValueObject obj)
        => obj.GetTypeAccount();

    public MethodResult<INotification> GetMethodResult()
        => MethodResult;
}
