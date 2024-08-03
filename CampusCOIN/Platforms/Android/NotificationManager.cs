using Android.App;
using Android.Content;
using Android.Runtime;
using AndroidX.Core.App;


namespace CampusCOIN.Services
{
    static partial class NotificationManager
    {
        private static Context Context;
        public const string CHANNEL_ID = "1";
        static NotificationManager()
        {
            //This constructor will run before the first call is made to DoSendNotification
            //It gets the NotificationManager for the android device and creates a channel
            //to send notifications on
            Context = Platform.CurrentActivity.ApplicationContext;
            var channelName = "CampusCoin Notification channel";
            var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.High);

            Android.App.NotificationManager notificationManager =
           Context.GetSystemService(Context.NotificationService)
            as Android.App.NotificationManager;
            notificationManager.CreateNotificationChannel(channel);
        }
        static partial void DoSendNotification(string title, string message, DateTime scheduleTime)
        {
            //Get the devices native alarm manager
            AlarmManager alarmManager = Context.GetSystemService(Context.AlarmService).JavaCast<AlarmManager>();
            int id = 1;
            var alarmIntent = new Intent(Context, typeof(AlarmReceiver));
            //Add extra tags so we can read it later
            alarmIntent.PutExtra("id", id);
            alarmIntent.PutExtra("title", title);
            alarmIntent.PutExtra("message", message);
            DateTime dateTime = scheduleTime;
            DateTimeOffset dateOffsetValue = DateTimeOffset.Parse(dateTime.ToString());
            long millisecondsToBegin = dateOffsetValue.ToUnixTimeMilliseconds();
            PendingIntent pending = PendingIntent.GetBroadcast(Context, id, alarmIntent,PendingIntentFlags.Immutable);
            //Schedule the alarm to trigger our AlarmReceiver at the designated time
            alarmManager.Set(AlarmType.RtcWakeup, millisecondsToBegin, pending);
        }
    }
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {
        //Receive the alarm broadcast
        public override void OnReceive(Context context, Intent intent)
        {
            System.Diagnostics.Debug.WriteLine($"intent: {intent}");

            //Get the extra data we added to the alarm earlier
            var title = intent.GetStringExtra("title");
            var message = intent.GetStringExtra("message");
            System.Diagnostics.Debug.WriteLine($"intent: {intent.Extras}");
            var id = intent.GetIntExtra("id", -1); // -1 is used as a default value if the key is not found




            //Specify that the notification should launch our app, and attach some custom data to read later
            Intent resultIntent = new Intent(context, typeof(MainActivity));
            //resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            //This adds extra data we could read when the notification is clicked,
            //such as the id of the model to be shown
            //resultIntent.PutExtra("test_key", "thisisatestkey");
            int pendingIntentId = DateTime.Now.Millisecond; // Using current time in milliseconds for uniqueness
            PendingIntent pendingIntent = PendingIntent.GetActivity(context, pendingIntentId, resultIntent, PendingIntentFlags.Immutable);

            var color = new Color(20, 50, 20).ToInt();
            //Build the notification
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context, NotificationManager.CHANNEL_ID)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetDefaults((int)(NotificationDefaults.Sound | NotificationDefaults.Vibrate))
                //This line references the image in our Resources folder we added previously
                .SetSmallIcon(Resource.Drawable.tracker)
                .SetColor(color)
                .SetContentIntent(pendingIntent)
                .SetPriority((int)NotificationPriority.High);

            Android.App.NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as Android.App.NotificationManager;
            Notification notification = builder.Build();
            //Call the notification

            notificationManager.Notify(id, notification);


        }
    }
}
