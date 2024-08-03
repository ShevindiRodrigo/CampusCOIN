using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;


namespace CampusCOIN.Services
{
    public static partial class NotificationManager
    {
        //Show a toast popup with the specified message
        public static async void ShowToast(string message)
        {
            CancellationTokenSource cancellationTokenSource
            = new CancellationTokenSource();
            //Specify the duration and font size
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(message, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }

        //Publicly available caller for DoSendNotification
        //(partial methods can't be public)
        public static void SendNotification(string title, string message, DateTime scheduledTime)
        {
            DoSendNotification(title, message, scheduledTime);
        }
        //Partial function signature to implement in platform-specific code
        static partial void DoSendNotification(string title, string message, DateTime scheduledTime);
    }
}
