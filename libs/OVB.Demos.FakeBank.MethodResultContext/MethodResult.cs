using OVB.Demos.FakeBank.MethodResultContext.Enumerators;
using OVB.Demos.FakeBank.MethodResultContext.Exceptions;
using OVB.Demos.FakeBank.NotificationContext.Interfaces;

namespace OVB.Demos.FakeBank.MethodResultContext;

public readonly struct MethodResult<TNotification>
    where TNotification : INotification
{
    public TypeMethodResult Result { get; }
    public bool IsSuccess => Result == TypeMethodResult.Success;
    public bool IsPartial => Result == TypeMethodResult.Partial;
    public bool IsFailure => Result == TypeMethodResult.Failure;
    public TNotification[] Notifications { get; }

    private MethodResult(
        TypeMethodResult result,
        TNotification[] notifications)
    {
        MethodResultException.ThrowExceptionIfTheMethodResultIsNotDefined(result);

        Result = result;
        Notifications = notifications;
    }

    public static MethodResult<TNotification> Build(
        TypeMethodResult result, TNotification[] notifications)
        => new(result, notifications);

    public static MethodResult<TNotification> BuildPartialResult(TNotification[] notifications)
        => Build(TypeMethodResult.Partial, notifications);
    public static MethodResult<TNotification> BuildFailureResult(TNotification[] notifications)
        => Build(TypeMethodResult.Failure, notifications);
    public static MethodResult<TNotification> BuildSuccessResult(TNotification[] notifications)
        => Build(TypeMethodResult.Success, notifications);

    public static MethodResult<TNotification> BuildFromAnotherMethodResult(MethodResult<TNotification> methodResult)
        => Build(methodResult.Result, methodResult.Notifications);
    public static MethodResult<TNotification> BuildFromAnothersMethodResults(params MethodResult<TNotification>[] methodResults)
    {
        var totalNotifications = 0;

        for (int i = 0; i < methodResults.Length; i++)
            totalNotifications += methodResults[i].Notifications.Length;

        TypeMethodResult? newMethodResultType = null; 
        var newMethodResultNotifications = new TNotification[totalNotifications];

        var lastNotificationAddedIndex = 0;

        for (int i = 0; i < methodResults.Length; i++)
        {
            var methodResult = methodResults[i];

            if (newMethodResultType == null)
                newMethodResultType = methodResult.Result;
            else if (newMethodResultType == TypeMethodResult.Success && methodResult.Result != TypeMethodResult.Success)
                newMethodResultType = methodResult.Result;

            for (int j = 0; j < methodResult.Notifications.Length; j++)
            {
                newMethodResultNotifications[lastNotificationAddedIndex] = methodResult.Notifications[j];

                lastNotificationAddedIndex++;
            }
        }

        return Build(
            result: newMethodResultType!.Value,
            notifications: newMethodResultNotifications);
    }
}
