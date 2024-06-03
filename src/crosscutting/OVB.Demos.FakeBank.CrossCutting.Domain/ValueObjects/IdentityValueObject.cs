
using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects.Exceptions;
using OVB.Demos.FakeBank.MethodResultContext;
using OVB.Demos.FakeBank.NotificationContext;
using OVB.Demos.FakeBank.NotificationContext.Interfaces;

namespace OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

public readonly struct IdentityValueObject
{
    public bool IsValid { get; }
    private Ulid IdentityId { get; }
    private MethodResult<INotification> MethodResult { get; }

    private IdentityValueObject(bool isValid, Ulid identityId, MethodResult<INotification> methodResult)
    {
        IsValid = isValid;
        IdentityId = identityId;
        MethodResult = methodResult;
    }

    private static INotification IdentityCannotBeNullOrEmptyOrMinValue(int? index = null) => Notification.BuildError(
        code: "IDENTITY_VALUE_OBJECT",
        message: "O ID de Identificação da Entidade não pode ser vazio ou inválido.",
        index: index);

    public static IdentityValueObject Build(Ulid? identityId = null, int? index = null)
    {
        if (identityId == null)
            return new IdentityValueObject(
                isValid: true,
                identityId: Ulid.NewUlid(),
                methodResult: MethodResult<INotification>.BuildSuccessResult(
                    notifications: []));

        if (identityId == Ulid.Empty || identityId == Ulid.MinValue)
            return new IdentityValueObject(
                isValid: false,
                identityId: Ulid.Empty,
                methodResult: MethodResult<INotification>.BuildFailureResult(
                    notifications: [IdentityCannotBeNullOrEmptyOrMinValue(index)]));

        return new IdentityValueObject(
                isValid: true,
                identityId: identityId!.Value,
                methodResult: MethodResult<INotification>.BuildSuccessResult(
                    notifications: []));
    }

    public static IdentityValueObject Build(Ulid identityId)
        => Build(identityId, null);

    public Ulid GetIdentityId()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return IdentityId;
    }

    public string GetIdentityIdAsString()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return IdentityId.ToString();
    }

    public override string ToString()
    {
        return IdentityId.ToString();
    }

    public static implicit operator MethodResult<INotification>(IdentityValueObject obj)
        => obj.GetMethodResult();

    public MethodResult<INotification> GetMethodResult()
        => MethodResult;
}
