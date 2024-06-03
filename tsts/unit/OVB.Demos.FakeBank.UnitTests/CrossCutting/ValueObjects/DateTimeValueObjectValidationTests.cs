using OVB.Demos.FakeBank.CrossCutting.Domain.ValueObjects;

namespace OVB.Demos.FakeBank.UnitTests.CrossCutting.ValueObjects;

public sealed class DateTimeValueObjectValidationTests
{
    [Fact]
    public void Should_Be_DateTime_Valid_When_Creating_A_Value_Object_With_Actual_Time()
    {
        // Arrange

        // Act
        var dateTime = DateTimeValueObject.BuildActualTime();

        // Assert
        Assert.True(dateTime.IsValid);
        Assert.Empty(dateTime.GetMethodResult().Notifications);
    }

    [Fact]
    public void Should_Be_DateTime_Equal_When_Creating_A_DateTime_Value_Object_With_An_Existing_Time()
    {
        // Arrange
        var dateTimeExample = DateTime.SpecifyKind(DateTime.Parse("2024-06-07 01:45:36"), DateTimeKind.Utc);
        const int brazilianTimeFromUtc = -3;

        // Act
        var dateTime = DateTimeValueObject.BuildUtcTime(dateTimeExample);

        // Assert
        Assert.Equal(dateTimeExample, dateTime);
        Assert.Equal(dateTimeExample.AddHours(brazilianTimeFromUtc), dateTime.GetBrazilianTime());
    }

    [Fact]
    public void Should_Be_Notification_Send_When_Creating_A_DateTime_Value_Object_With_A_Not_Expected_Specify_Kind_Time()
    {
        // Arrange
        var dateTimeExample = DateTime.Parse("2024-06-07 01:45:36");
        var index = 0;

        const string codeMessageExpected = "DATETIME_VALUE_OBJECT_SPECIFY_KIND";
        const string messageExpected = "O horário precisa estar na especificação e padrão UTC.";

        // Act
        var dateTime = DateTimeValueObject.BuildUtcTime(dateTimeExample, index);

        // Assert
        Assert.False(dateTime.IsValid);
        Assert.Single(dateTime.GetMethodResult().Notifications);
        Assert.False(dateTime.GetMethodResult().IsSuccess);
        Assert.Equal(
            expected: codeMessageExpected,
            actual: dateTime.GetMethodResult().Notifications[0].Code);
        Assert.Equal(
            expected: messageExpected,
            actual: dateTime.GetMethodResult().Notifications[0].Message);
        Assert.Equal(
            expected: index,
            actual: dateTime.GetMethodResult().Notifications[0].Index);
        Assert.Equal(
            expected: dateTime.GetMethodResult(),
            actual: dateTime);
    }
}
