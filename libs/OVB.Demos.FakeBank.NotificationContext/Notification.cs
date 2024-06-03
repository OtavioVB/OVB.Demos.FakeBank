using OVB.Demos.FakeBank.NotificationContext.Enumerators;
using OVB.Demos.FakeBank.NotificationContext.Interfaces;

namespace OVB.Demos.FakeBank.NotificationContext;

public class Notification : INotification
{
    public string Code { get; }
    public string Message { get; }
    public string Type { get; }
    public int? Index { get; }

    private Notification(string code, string message, string type, int? index = null)
    {
        Code = code;
        Message = message;
        Type = type;
        Index = index;
    }

    public static Notification Build(
        string code, string message, TypeNotification type, int? index = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentNullException(nameof(code));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentNullException(nameof(message));

        if (!Enum.IsDefined(type))
            throw new ArgumentOutOfRangeException(nameof(type));

        return new(code, message, type.ToString(), index);
    }

    public static Notification BuildSuccess(string code, string message, int? index = null)
        => Build(code, message, TypeNotification.Success, index);

    public static Notification BuildInformation(string code, string message, int? index = null)
        => Build(code, message, TypeNotification.Information, index);

    public static Notification BuildError(string code, string message, int? index = null)
        => Build(code, message, TypeNotification.Error, index);
}
