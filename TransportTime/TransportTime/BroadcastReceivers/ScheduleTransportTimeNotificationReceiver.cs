using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Icu.Util;
using Android.Widget;
using TransportTime.Models;
using TransportTime.Notifications;
using XamarinCustomHelper.IO;
using XamarinCustomHelper.Phone;

namespace TransportTime.BroadcastReceivers
{
    [BroadcastReceiver(Enabled = true, Permission = "RECEIVE_BOOT_COMPLETED", Exported = true)]
    [IntentFilter(new string[] {
        Intent.ActionBootCompleted,
        "android.intent.action.QUICKBOOT_POWERON",
        Intent.ActionLockedBootCompleted })]
    public class ScheduleTransportTimeNotificationReceiver : BroadcastReceiver
    {
        private const int RequestId = 123;
        public override void OnReceive(Context context, Intent intent)
        {
            var notificationScheduler = new NotificationScheduler(context);
            notificationScheduler.ScheduleTransportTimeNotifications();
            ScheduleDailyChecker(context);
        }

        private void ScheduleDailyChecker(Context context)
        {
            Intent scheduleIntent = new Intent(context, typeof(ScheduleTransportTimeNotificationReceiver));
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, RequestId, scheduleIntent, 0);

            var tomorrow = DateTime.Now.AddDays(1);
            tomorrow = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, 0, 30);
            var timeSpan = tomorrow - DateTime.Now;

            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            alarmManager.Cancel(pendingIntent);
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, Java.Lang.JavaSystem.CurrentTimeMillis() + (long)timeSpan.TotalMilliseconds, pendingIntent);
        }
    }
}
