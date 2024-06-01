using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

namespace OVB.Demos.FakeBank.UnitTests.CrossCutting.ValueObjects;

public sealed class IdentityValueObjectValidationTests
{
    [Fact]
    public void Should_Be_Valid_Identity_Value_Object_Creation()
    {
        // Arrange 

        // Act
        var identity = IdentityValueObject.Build();

        // Assert
        Assert.True(identity.IsValid);
        Assert.Empty(identity.GetMethodResult().Notifications);
        Assert.True(identity.GetMethodResult().IsSuccess);
    }

    [Fact]
    public void Should_Be_Not_Valid_Identity_Value_Object_Creation_When_Send_An_Empty_Ulid()
    {
        // Arrange
        var identityEmpty = Ulid.Empty;

        // Act
        var identity = IdentityValueObject.Build(identityEmpty);

        // Assert
        Assert.False(identity.IsValid);
        Assert.NotEmpty(identity.GetMethodResult().Notifications);
        Assert.True(identity.GetMethodResult().IsFailure);
    }

    [Fact]
    public void Should_Be_Not_Valid_Identity_Value_Object_Creation_When_Send_An_MinValue_Ulid()
    {
        // Arrange
        var identityEmpty = Ulid.MinValue;

        // Act
        var identity = IdentityValueObject.Build(identityEmpty);

        // Assert
        Assert.False(identity.IsValid);
        Assert.NotEmpty(identity.GetMethodResult().Notifications);
        Assert.True(identity.GetMethodResult().IsFailure);
    }

    [Fact]
    public void Should_Be_Get_Identity_Value_Methods_Equal_As_Expected()
    {
        // Arrange
        var identityUlid = Ulid.NewUlid();

        // Act
        var identity = IdentityValueObject.Build(
            identityId: identityUlid);

        // Assert
        Assert.Equal(
            expected: identityUlid,
            actual: identity.GetIdentityId());
        Assert.Equal(
            expected: identityUlid.ToString(),
            actual: identity.GetIdentityIdAsString());
        Assert.Equal(
            expected: identityUlid.ToString(),
            actual: identity.ToString());
    }

    [Fact]
    public void Should_Be_Notification_As_Expected_When_The_Identity_Value_Object_Is_Not_Valid()
    {
        // Arrange
        var identityEmpty = Ulid.Empty;
        var index = 0;

        // Act
        var identity = IdentityValueObject.Build(identityEmpty);
        var identityWithIndex = IdentityValueObject.Build(identityEmpty, index);

        // Assert
        Assert.Single(identity.GetMethodResult().Notifications);
        Assert.Equal(
            expected: "IDENTITY_VALUE_OBJECT",
            actual: identity.GetMethodResult().Notifications[0].Code);
        Assert.Equal(
            expected: "O ID de Identificação da Entidade não pode ser vazio ou inválido.",
            actual: identity.GetMethodResult().Notifications[0].Message);
        Assert.Null(identity.GetMethodResult().Notifications[0].Index);
        Assert.Equal(
            expected: index,
            actual: identityWithIndex.GetMethodResult().Notifications[0].Index);
    }
}
