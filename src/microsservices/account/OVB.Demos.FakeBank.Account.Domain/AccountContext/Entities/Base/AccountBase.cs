using OVB.Demos.FakeBank.Account.Domain.AccountContext.Enumerators;
using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

namespace OVB.Demos.FakeBank.Account.Domain.AccountContext.Entities.Base;

public abstract class AccountBase
{
    protected readonly TypeAccount _typeAccount;

    protected AccountBase(TypeAccount typeAccount)
    {
        _typeAccount = typeAccount;
    }

    public IdentityValueObject AccountId { get; protected set; }
    public DateTimeValueObject CreatedAt { get; protected set; }
    public DateTimeValueObject LastModifiedAt { get; protected set; }
}
