using OVB.Demos.FakeBank.Account.Domain.AccountContext.ValueObjects;
using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

namespace OVB.Demos.FakeBank.Account.Domain.AccountContext.Functions.CreateBasicAccount.Inputs;

public readonly struct CreatePreviousRegisterDomainFunctionInput
{
    public LegalNameValueObject LegalName { get; }
    public DocumentValueObject Document { get; }
}
