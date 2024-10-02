using BudgetPlanner.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Sentry;

namespace BudgetPlanner.Handlers
{
    public static class ErrorHandler
    {
        public static void HandleError<T>(T exception) where T : Exception
        {
            SentrySdk.CaptureException(exception);
            WeakReferenceMessenger.Default.Send(new ErrorOccuredMessage(true));
        }
    }
}
