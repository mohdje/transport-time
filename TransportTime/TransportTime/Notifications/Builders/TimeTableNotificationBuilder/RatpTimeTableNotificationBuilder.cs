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
    public class RatpTimeTableNotificationBuilder : TimeTableNotificationBuilder
    {
        private int[] _destinationTextViewIds = new int[] { Resource.Id.DestinationLine1, Resource.Id.DestinationLine2, Resource.Id.DestinationLine3, Resource.Id.DestinationLine4 };
        private int[] _timeTextViewIds = new int[] { Resource.Id.TimeLine1, Resource.Id.TimeLine2, Resource.Id.TimeLine3, Resource.Id.TimeLine4 };

        public RatpTimeTableNotificationBuilder(Context context) : base(context)
        {

        }

        protected override int BigViewLayoutId()
        {
            return Resource.Layout.BigTimeNotificationView;
        }

        protected override void FillTimeTable(RemoteViews notificationView, NextPassageLite[] timeTable)
        {
            for (int i = 0; i < _destinationTextViewIds.Length; i++)
            {
                if (i <= timeTable.Length - 1)
                {
                    notificationView.SetViewVisibility(_destinationTextViewIds[i], ViewStates.Visible);
                    notificationView.SetViewVisibility(_timeTextViewIds[i], ViewStates.Visible);

                    var destination = timeTable[i].DirectionName.Length > 12 ? timeTable[i].DirectionName.Substring(0, 12) + "..." : timeTable[i].DirectionName;

                    notificationView.SetTextViewText(_destinationTextViewIds[i], destination);
                    notificationView.SetTextViewText(_timeTextViewIds[i], timeTable[i].RemainingTime);
                }
                else
                {
                    notificationView.SetViewVisibility(_destinationTextViewIds[i], ViewStates.Gone);
                    notificationView.SetViewVisibility(_timeTextViewIds[i], ViewStates.Gone);
                }
            }
        }

    }
}