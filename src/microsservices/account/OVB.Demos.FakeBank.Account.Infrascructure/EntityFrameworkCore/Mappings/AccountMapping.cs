using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OVB.Demos.FakeBank.Account.Domain.AccountContext.DataModels;
using OVB.Demos.FakeBank.Account.Domain.AccountContext.ValueObjects;
using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;
using System.Globalization;

namespace OVB.Demos.FakeBank.Account.Infrascructure.EntityFrameworkCore.Mappings;

public sealed class AccountMapping : IEntityTypeConfiguration<AccountDataModel>
{
    public void Configure(EntityTypeBuilder<AccountDataModel> builder)
    {
        #region Table Configuration

        builder.ToTable(
            name: "account",
            schema: "accounts");

        #endregion

        #region Primary Key Configuration

        builder.HasKey(p => p.AccountId)
            .HasName("pk_account_id");

        #endregion

        #region Foreign Key Configuration



        #endregion

        #region Index Key Configuration

        builder.HasIndex(p => p.AccountId)
            .IsUnique(true)
            .HasDatabaseName("uk_account_identity_id");

        #endregion

        #region Properties Configuration

        builder.Property(p => p.AccountId)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnType("VARCHAR")
            .HasMaxLength(Ulid.NewUlid().ToString().Length)
            .HasColumnName("idaccount")
            .HasConversion(p => p.GetIdentityIdAsString(), p => IdentityValueObject.Build(Ulid.Parse(p)))
            .ValueGeneratedNever();
        builder.Property(p => p.ObfuscatedId)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasMaxLength(512)
            .HasColumnName("idaccount_obfuscated")
            .HasConversion(p => p.GetIdentityObfuscatedToBase64String(), p => IdentityObfuscatedValueObject.Build(p))
            .ValueGeneratedNever();

        builder.Property(p => p.CreatedAt)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("TIMESTAMPTZ")
            .HasColumnName("created_at")
            .HasConversion(p => p.GetDateTime(), p => DateTimeValueObject.BuildUtcTime(DateTime.SpecifyKind(p, DateTimeKind.Utc)))
            .ValueGeneratedNever();
        builder.Property(p => p.Type)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasMaxLength(255)
            .HasColumnName("type")
            .HasConversion(p => p.GetTypeAccountToString(), p => AccountTypeValueObject.Build(p))
            .ValueGeneratedNever();
        builder.Property(p => p.Status)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasMaxLength(255)
            .HasColumnName("status")
            .HasConversion(p => p.GetAccountStatusToString(), p => AccountStatusValueObject.Build(p))
            .ValueGeneratedNever();
        builder.Property(p => p.Document)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasMaxLength(14)
            .HasColumnName("document")
            .HasConversion(p => p.GetDocument(), p => DocumentValueObject.Build(p))
            .ValueGeneratedNever();
        builder.Property(p => p.LegalName)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasMaxLength(14)
            .HasColumnName("legalName")
            .HasConversion(p => p.GetLegalName(), p => LegalNameValueObject.Build(p))
            .ValueGeneratedNever();
        builder.Property(p => p.LastModifiedAt)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("TIMESTAMPTZ")
            .HasColumnName("last_modified_at")
            .HasConversion(p => p.GetDateTime(), p => DateTimeValueObject.BuildUtcTime(DateTime.SpecifyKind(p, DateTimeKind.Utc)))
            .ValueGeneratedNever();

        #endregion
    }
}
