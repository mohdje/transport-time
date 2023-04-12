using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransportTime.BroadcastReceivers;
using TransportTime.Models;
using XamarinCustomHelper.IO;
using XamarinCustomHelper.Phone;

namespace TransportTime.Notifications
{
    class NotificationScheduler
    {
        Context context;
        public NotificationScheduler(Context context)
        {
            this.context = context;
        }
        public void ScheduleTransportTimeNotifications()
        {
            var scheduledNotifs = GetScheduledTransportTimeNotification(context);

            var todaysNotifications = scheduledNotifs.Where(s => s.ScheduledTime.Day.Equals(DateTime.Now.DayOfWeek));

            foreach (var notif in todaysNotifications)
            {
                var notificationTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, notif.ScheduledTime.Hours, notif.ScheduledTime.Minutes, 0);
                var timeSpan = notificationTime - DateTime.Now;

                if (timeSpan.TotalMilliseconds > 0)
                    ScheduleNotification(notif, timeSpan);
            }
        }

        public void CancelTransportTimeNotification(ScheduledNotification scheduledTransportTimeNotification)
        {
            PendingIntent pendingIntent = BuildNotificationPendingIntent(scheduledTransportTimeNotification);

            AlarmManager alarmManager = (AlarmManager)this.context.GetSystemService(Context.AlarmService);
            alarmManager.Cancel(pendingIntent);
        }
        private void ScheduleNotification(ScheduledNotification scheduledTransportTimeNotification, TimeSpan delay)
        {
            PendingIntent pendingIntent = BuildNotificationPendingIntent(scheduledTransportTimeNotification);

            AlarmManager alarmManager = (AlarmManager)this.context.GetSystemService(Context.AlarmService);
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, Java.Lang.JavaSystem.CurrentTimeMillis() + (long)delay.TotalMilliseconds, pendingIntent);
        }

        private PendingIntent BuildNotificationPendingIntent(ScheduledNotification scheduledTransportTimeNotification)
        {
            Intent broadcastReceiverIntent = new Intent(context, typeof(ShowTransportTimeNotificationReceiver));

            broadcastReceiverIntent.PutExtra("transportType", scheduledTransportTimeNotification.TransportStopData.Type);
            broadcastReceiverIntent.PutExtra("lineId", scheduledTransportTimeNotification.TransportStopData.LineId);
            broadcastReceiverIntent.PutExtra("lineName", scheduledTransportTimeNotification.TransportStopData.LineName);
            broadcastReceiverIntent.PutExtra("stopAreaId", scheduledTransportTimeNotification.TransportStopData.StopAreaId);
            broadcastReceiverIntent.PutExtra("stopAreaName", scheduledTransportTimeNotification.TransportStopData.StopAreaName);
            broadcastReceiverIntent.PutExtra("destinationStopAreaId", scheduledTransportTimeNotification.TransportStopData.DestinationStopAreaId);
            broadcastReceiverIntent.PutExtra("destinationStopAreaName", scheduledTransportTimeNotification.TransportStopData.DestinationStopAreaName);

            return PendingIntent.GetBroadcast(context, scheduledTransportTimeNotification.GetId(), broadcastReceiverIntent, 0);
        }

        private ScheduledNotification[] GetScheduledTransportTimeNotification(Context context)
        {
            ScheduledNotificationList scheduledPinList = null;
            try
            {
                scheduledPinList = new ScheduledNotificationList(new AndroidXmlSerializer(context));
            }
            catch (Exception)
            {
            }

            return scheduledPinList?.Elements ?? new ScheduledNotification[] { };
        }

       
    }
}