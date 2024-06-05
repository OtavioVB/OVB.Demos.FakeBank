using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

namespace OVB.Demos.FakeBank.UnitTests.ValueObjects;

public sealed class DocumentValueObjectValidationTests
{
    [Theory]
    [InlineData("11111111111")]
    [InlineData("01234567891234")]
    public void Should_Be_Document_Valid_As_Expected(string obj)
    {
        // Arrange

        // Act
        var document = DocumentValueObject.Build(obj);

        // Assert
        Assert.True(document.IsValid);
        Assert.True(document.GetMethodResult().IsSuccess);
        Assert.Empty(document.GetMethodResult().Notifications);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("1111111X11K")]
    [InlineData("01234567  1234")]
    public void Should_Be_Document_Is_Not_Valid(string obj)
    {
        // Arrange

        // Act
        var document = DocumentValueObject.Build(obj);

        // Assert
        Assert.False(document.IsValid);
        Assert.True(document.GetMethodResult().IsFailure);
        Assert.Single(document.GetMethodResult().Notifications);
    }
}
