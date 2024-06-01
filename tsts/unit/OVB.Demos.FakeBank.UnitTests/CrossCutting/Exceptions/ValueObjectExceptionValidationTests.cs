using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects.Exceptions;

namespace OVB.Demos.FakeBank.UnitTests.CrossCutting.Exceptions;

public sealed class ValueObjectExceptionValidationTests
{
    [Fact]
    public void Should_Method_Throw_Exception_When_Resource_Send_False()
    {
        // Arrange
        var resourceIsValid = false;

        // Act
        var action = () => { ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(resourceIsValid); };

        // Assert
        Assert.Throws<ValueObjectException>(action);
    }
}
