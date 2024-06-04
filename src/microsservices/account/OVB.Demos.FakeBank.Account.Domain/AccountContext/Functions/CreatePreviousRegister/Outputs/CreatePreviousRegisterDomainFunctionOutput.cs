using OVB.Demos.FakeBank.Account.Domain.AccountContext.ValueObjects;
using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

namespace OVB.Demos.FakeBank.Account.Domain.AccountContext.Functions.CreateBasicAccount.Outputs;

public readonly struct CreatePreviousRegisterDomainFunctionOutput
{
    public IdentityValueObject AccountId { get; }
    public OffuscationTokenValueObject AccountSecret { get; }
    public DateTimeValueObject CreatedAt { get; }
    public AccountStatusValueObject Status { get; }
    public DateTimeValueObject LastModifiedAt { get; }
}
