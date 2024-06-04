using OVB.Demos.FakeBank.Account.Domain.AccountContext.ValueObjects;
using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

namespace OVB.Demos.FakeBank.Account.Domain.AccountContext.DataModels;

public sealed record AccountDataModel
{
    public AccountDataModel(
        IdentityValueObject accountId, 
        IdentityObfuscatedValueObject obfuscatedId,
        DateTimeValueObject createdAt,
        AccountTypeValueObject type,
        AccountStatusValueObject status,
        DateTimeValueObject lastModifiedAt)
    {
        AccountId = accountId;
        ObfuscatedId = obfuscatedId;
        CreatedAt = createdAt;
        Type = type;
        Status = status;
        LastModifiedAt = lastModifiedAt;
    }

    public IdentityValueObject AccountId { get; set; }
    public IdentityObfuscatedValueObject ObfuscatedId { get; set; }
    public DateTimeValueObject CreatedAt { get; set; }
    public AccountTypeValueObject Type { get; set; }
    public AccountStatusValueObject Status { get; set; }
    public DateTimeValueObject LastModifiedAt { get; set; }
}
