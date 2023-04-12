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
using TransportTime.BroadcastReceivers;
using TransportTime.Models;
using TransportTime.Extensions;

namespace TransportTime.Notifications.Builders
{
    public abstract class TimeTableNotificationBuilder : NotificationBuilder
    {
        public TimeTableNotificationBuilder(Context context) : base(context)
        {

        }

        protected override RemoteViews BuildSmallView(NotificationData data)
        {
            RemoteViews smallView = new RemoteViews(this.Context.PackageName, Resource.Layout.SmallTimeNotificationView);
            smallView.SetTextViewText(Resource.Id.lineAndStation, data.Title);
            smallView.SetTextViewText(Resource.Id.notificationInfo, data.ExpandMessage);

            return smallView;
        }
        protected override RemoteViews BuildBigView(NotificationData data)
        {
            RemoteViews bigView = new RemoteViews(this.Context.PackageName, BigViewLayoutId());
            bigView.SetTextViewText(Resource.Id.lineAndStation, data.Title);

            if (data.NextPassages != null && data.NextPassages.Length > 0)
            {
                bigView.SetViewVisibility(Resource.Id.timeTableGridLayout, ViewStates.Visible);
                bigView.SetViewVisibility(Resource.Id.noResultsTextView, ViewStates.Gone);

                FillTimeTable(bigView, data.NextPassages);
            }
            else
            {
                bigView.SetViewVisibility(Resource.Id.timeTableGridLayout, ViewStates.Gone);
                bigView.SetViewVisibility(Resource.Id.noResultsTextView, ViewStates.Visible);
            }

            Intent broadcastReceiverIntent = new Intent(this.Context, typeof(ShowTransportTimeNotificationReceiver));
            broadcastReceiverIntent.PutExtra("transportType", data.TransportStopData.Type);
            broadcastReceiverIntent.PutExtra("lineId", data.TransportStopData.LineId);
            broadcastReceiverIntent.PutExtra("lineName", data.TransportStopData.LineName);
            broadcastReceiverIntent.PutExtra("stopAreaId", data.TransportStopData.StopAreaId);
            broadcastReceiverIntent.PutExtra("stopAreaName", data.TransportStopData.StopAreaName);
            broadcastReceiverIntent.PutExtra("destinationStopAreaId", data.TransportStopData.DestinationStopAreaId);
            broadcastReceiverIntent.PutExtra("destinationStopAreaName", data.TransportStopData.DestinationStopAreaName);

            PendingIntent btPendingIntent = PendingIntent.GetBroadcast(this.Context, this.NotificationId, broadcastReceiverIntent, 0);

            bigView.SetOnClickPendingIntent(Resource.Id.BtnUpdate, btPendingIntent);

            return bigView;
        }

        protected abstract void FillTimeTable(RemoteViews notificationView, NextPassageLite[] timeTable);
        protected abstract int BigViewLayoutId();

    }
}