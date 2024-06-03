using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

namespace OVB.Demos.FakeBank.Account.Domain.AccountContext.DataModels;

public sealed record AccountDataModel
{
    public AccountDataModel(IdentityValueObject accountId, DateTimeValueObject createdAt, DateTimeValueObject lastModifiedAt)
    {
        AccountId = accountId;
        CreatedAt = createdAt;
        LastModifiedAt = lastModifiedAt;
    }

    public IdentityValueObject AccountId { get; set; }
    public DateTimeValueObject CreatedAt { get; set; }
    public DateTimeValueObject LastModifiedAt { get; set; }
}
