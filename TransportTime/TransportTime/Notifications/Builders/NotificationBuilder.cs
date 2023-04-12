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
using TransportTime.Models;
using TransportTime.Extensions;

namespace TransportTime.Notifications.Builders
{
    public abstract class NotificationBuilder
    {
        protected Context Context { get; }

        protected int NotificationId { get; set; }
        public NotificationBuilder(Context context)
        {
            Context = context;
        }
        public NotificationWrapper BuildNotification(string channelId, NotificationData data)
        {
            NotificationId = data.TransportStopData.Hash;

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this.Context, channelId)
                            .SetSmallIcon(Resource.Drawable.notification_icon)
                            .SetCustomContentView(BuildSmallView(data))
                            .SetCustomBigContentView(BuildBigView(data))
                            .SetPriority((int)NotificationPriority.High);
            
            return new NotificationWrapper(NotificationId, builder.Build());
        }

        protected abstract RemoteViews BuildSmallView(NotificationData data);
        protected abstract RemoteViews BuildBigView(NotificationData data);
    }
}