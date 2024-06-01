using System.Text.Json.Serialization;

namespace OVB.Demos.FakeBank.NotificationContext.Interfaces;

public interface INotification
{
    public string Code { get; }
    public string Message { get; }
    public string Type { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Index { get; }
}
