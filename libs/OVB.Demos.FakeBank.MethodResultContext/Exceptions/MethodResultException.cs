using OVB.Demos.FakeBank.MethodResultContext.Enumerators;

namespace OVB.Demos.FakeBank.MethodResultContext.Exceptions;

public sealed class MethodResultException : Exception
{
    public MethodResultException(string message) : base(message)
    {
    }

    public static void ThrowExceptionIfTheMethodResultIsNotDefined(TypeMethodResult type)
    {
        if (!Enum.IsDefined(type))
            throw new MethodResultException("The enumerator about type of method result is not defined.");
    }
}
