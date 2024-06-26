﻿using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects.Exceptions;
using OVB.Demos.FakeBank.MethodResultContext;
using OVB.Demos.FakeBank.NotificationContext;
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

    private static INotification DateTimeCannotSpecifiedWithoutUtc(int? index = null) => Notification.BuildError(
        code: "DATETIME_VALUE_OBJECT_SPECIFY_KIND",
        message: "O horário precisa estar na especificação e padrão UTC.",
        index: index);

    public static DateTimeValueObject BuildUtcTime(DateTime utcTime, int? index = null)
    {
        if (utcTime.Kind != DateTimeKind.Utc)
            return new DateTimeValueObject(
                isValid: false,
                dateTime: DateTime.MinValue,
                methodResult: MethodResult<INotification>.BuildFailureResult(
                    notifications: [DateTimeCannotSpecifiedWithoutUtc(index)]));

        return new(
            isValid: true,
            dateTime: utcTime,
            methodResult: MethodResult<INotification>.BuildSuccessResult(
                notifications: []));
    }

    public static DateTimeValueObject BuildUtcTime(DateTime utcTime)
        => BuildUtcTime(utcTime);

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
