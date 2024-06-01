namespace OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects.Exceptions;


public sealed class ValueObjectException : Exception
{
    public ValueObjectException(string message) : base(message)
    {
    }

    public static void ThrowExceptionIfTheResourceIsNotValid(bool isValid)
    {
        if (!isValid)
            throw new ValueObjectException("The resource that you want's to get is not valid.");
    }
}
