using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects.Exceptions;
using OVB.Demos.FakeBank.MethodResultContext;
using OVB.Demos.FakeBank.NotificationContext;
using OVB.Demos.FakeBank.NotificationContext.Interfaces;
using System.Globalization;

namespace OVB.Demos.FakeBank.Account.Domain.AccountContext.ValueObjects;

public readonly struct LegalNameValueObject
{
    public bool IsValid { get; }
    private string LegalName { get; }
    private MethodResult<INotification> MethodResult { get; }

    private LegalNameValueObject(bool isValid, string legalName, MethodResult<INotification> methodResult)
    {
        IsValid = isValid;
        LegalName = legalName;
        MethodResult = methodResult;
    }

    public const int MaxLength = 128;

    private static INotification LegalNameCannotBeEmpty(int? index = null)
        => Notification.BuildError(
            code: "LEGAL_NAME_VALUE_OBJECT_EMPTY",
            message: "O nome legal do proprietário da conta corrente não pode ser vazia",
            index: index);

    private static INotification LegalNameCannotBeGreaterThanMaxLength(int? index = null)
        => Notification.BuildError(
            code: "LEGAL_NAME_VALUE_OBJECT_MAX_LENGTH",
            message: $"O nome legal do proprietário da conta corrente não pode conter mais que {MaxLength} caracteres.",
            index: index);

    public static LegalNameValueObject Build(
        string legalName, int? index = null)
    {
        var notifications = new List<INotification>(2);

        if (string.IsNullOrWhiteSpace(legalName))
            notifications.Add(LegalNameCannotBeEmpty(index));

        if (legalName.Length > MaxLength)
            notifications.Add(LegalNameCannotBeGreaterThanMaxLength(index));

        if (notifications.Count > 0)
            return new LegalNameValueObject(
                isValid: false,
                legalName: string.Empty,
                methodResult: MethodResult<INotification>.BuildFailureResult(
                    notifications: notifications.ToArray()));

        return new LegalNameValueObject(
            isValid: true,
            legalName: CultureInfo.GetCultureInfo("pt-br").TextInfo.ToTitleCase(legalName),
            methodResult: MethodResult<INotification>.BuildSuccessResult());
    }

    public string GetLegalName()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return LegalName;
    }

    public static implicit operator string(LegalNameValueObject obj)
        => obj.GetLegalName();
    public static implicit operator MethodResult<INotification>(LegalNameValueObject obj)
        => obj.GetMethodResult();
    public static implicit operator LegalNameValueObject(string obj)
        => Build(obj);

    public MethodResult<INotification> GetMethodResult()
        => MethodResult;
}
