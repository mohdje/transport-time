using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TransportTime.Notifications
{
    public class NotificationWrapper
    {
        public Notification Notification { get; }
        public int NotificationId { get; }

        public NotificationWrapper(int notificationId, Notification notification)
        {
            NotificationId = notificationId;
            Notification = notification;
        }
    }
}