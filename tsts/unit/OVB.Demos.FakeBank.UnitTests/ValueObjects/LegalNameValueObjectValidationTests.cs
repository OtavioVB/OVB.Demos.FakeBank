using OVB.Demos.FakeBank.Account.Domain.AccountContext.ValueObjects;

namespace OVB.Demos.FakeBank.UnitTests.ValueObjects;

public sealed class LegalNameValueObjectValidationTests
{
    [Theory]
    [InlineData("FAKE BANK INTERMEDIACAO BANCARIA LTDA")]
    [InlineData("NOME DA PESSOA SILVA")]
    [InlineData("TESTE DO NOME DA PESSOA")]
    public void Should_Be_Legal_Name_Value_Object_Valid(string legalName)
    {
        // Arrange

        // Act
        var legalNameValueObject = LegalNameValueObject.Build(legalName);

        // Assert
        Assert.True(legalNameValueObject.IsValid);
        Assert.True(legalNameValueObject.GetMethodResult().IsSuccess);
        Assert.Empty(legalNameValueObject.GetMethodResult().Notifications);
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789")]
    public void Should_Be_Legal_Name_Value_Object_Not_Valid(string legalName)
    {
        // Arrange

        // Act
        var legalNameValueObject = LegalNameValueObject.Build(legalName);

        // Assert
        Assert.False(legalNameValueObject.IsValid);
        Assert.False(legalNameValueObject.GetMethodResult().IsSuccess);
        Assert.Single(legalNameValueObject.GetMethodResult().Notifications);
    }


}
