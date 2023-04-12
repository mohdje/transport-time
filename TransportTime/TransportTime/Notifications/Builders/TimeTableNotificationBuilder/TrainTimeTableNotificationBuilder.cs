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
    public class TrainTimeTableNotificationBuilder : TimeTableNotificationBuilder
    {
        private int[] _trainCodeViewIds = new int[] { Resource.Id.TrainCodeLine1, Resource.Id.TrainCodeLine2, Resource.Id.TrainCodeLine3, Resource.Id.TrainCodeLine4 };
        private int[] _timeTextViewIds = new int[] { Resource.Id.TimeLine1, Resource.Id.TimeLine2, Resource.Id.TimeLine3, Resource.Id.TimeLine4 };
        public TrainTimeTableNotificationBuilder(Context context) : base(context)
        {

        }
        protected override int BigViewLayoutId()
        {
            return Resource.Layout.BigTimeTrainNotificationView;
        }

        protected override void FillTimeTable(RemoteViews notificationView, NextPassageLite[] timeTable)
        {
            for (int i = 0; i < _trainCodeViewIds.Length; i++)
            {
                if (i <= timeTable.Length - 1)
                {
                    notificationView.SetViewVisibility(_trainCodeViewIds[i], ViewStates.Visible);
                    notificationView.SetViewVisibility(_timeTextViewIds[i], ViewStates.Visible);

                    notificationView.SetTextViewText(_trainCodeViewIds[i], timeTable[i].CodeMission);
                    notificationView.SetTextViewText(_timeTextViewIds[i], timeTable[i].RemainingTime);
                }
                else
                {
                    notificationView.SetViewVisibility(_trainCodeViewIds[i], ViewStates.Gone);
                    notificationView.SetViewVisibility(_timeTextViewIds[i], ViewStates.Gone);
                }
            }
        }
    }
}