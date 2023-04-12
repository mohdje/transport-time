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
using TransportTime.Models;

namespace TransportTime.Notifications.Builders
{
    public class LoadingTimeTableNotificationBuilder : NotificationBuilder
    {
        public LoadingTimeTableNotificationBuilder(Context context) : base(context)
        {

        }
        protected override RemoteViews BuildBigView(NotificationData data)
        {
            return BuildSmallView(data);
        }


        protected override RemoteViews BuildSmallView(NotificationData data)
        {
            RemoteViews smallView = new RemoteViews(this.Context.PackageName, Resource.Layout.SmallTimeNotificationView);
            smallView.SetTextViewText(Resource.Id.lineAndStation, data.Title);
            smallView.SetTextViewText(Resource.Id.notificationInfo, data.LoadingMessage);

            return smallView;
        }
    }
}