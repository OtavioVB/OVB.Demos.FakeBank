using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects.Exceptions;
using OVB.Demos.FakeBank.MethodResultContext;
using OVB.Demos.FakeBank.NotificationContext.Interfaces;

namespace OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

public readonly struct DateTimeValueObject
{
    public bool IsValid { get; }
    private DateTime DateTime { get; }
    private MethodResult<INotification> MethodResult { get; }

    private DateTimeValueObject(bool isValid, DateTime dateTime, MethodResult<INotification> methodResult)
    {
        IsValid = isValid;
        DateTime = dateTime;
        MethodResult = methodResult;
    }

    public const int BrazilianTimeHoursFromUtc = -3;

    public static DateTimeValueObject BuildActualTime()
        => new(
            isValid: true, 
            dateTime: DateTime.UtcNow, 
            methodResult: MethodResult<INotification>.BuildSuccessResult(
                notifications: []));

    public static DateTimeValueObject BuildTime(DateTime utcTime)
        => new(
            isValid: true,
            dateTime: utcTime, 
            methodResult: MethodResult<INotification>.BuildSuccessResult(
                notifications: []));

    public DateTime GetDateTime()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return DateTime;
    }

    public DateTime GetBrazilianTime()
    {
        ValueObjectException.ThrowExceptionIfTheResourceIsNotValid(IsValid);

        return DateTime.AddHours(BrazilianTimeHoursFromUtc);
    }

    public static implicit operator DateTime(DateTimeValueObject obj)
        => obj.GetDateTime();
    public static implicit operator MethodResult<INotification>(DateTimeValueObject obj)
        => obj.GetMethodResult();

    public MethodResult<INotification> GetMethodResult()
        => MethodResult;
}
