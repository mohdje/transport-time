using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransportTime.Notifications
{
    public class NotificationPusher
    {
        public const string ChannelId = "TransportTimeNotificationChannel";

        private const string channelName = "TransportTime Notifications";

        private const string channelDescription = "Channel for TransportTime notifications";

        private static bool channelCreated;

        private Context context;
        public NotificationPusher(Context context)
        {
            this.context = context;

            if (!channelCreated)
            {
                if (Build.VERSION.SdkInt < BuildVersionCodes.O)
                {
                    // Notification channels are new in API 26 (and not a part of the
                    // support library). There is no need to create a notification
                    // channel on older versions of Android.
                    return;
                }

                var channel = new NotificationChannel(ChannelId, channelName, NotificationImportance.High)
                {
                    Description = channelDescription
                };

                channel.EnableVibration(true);

                channel.SetShowBadge(true);

                var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);

                notificationManager.CreateNotificationChannel(channel);
                channelCreated = true;
            }
        }

        public void PushNotification(NotificationWrapper notificationWrapper)
        {
            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(notificationWrapper.NotificationId, notificationWrapper.Notification);
        }
    }
}