using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TransportTime.BroadcastReceivers;
using TransportTime.Models;
using TransportTime.Notifications.Builders;
using Android.Support.V4.App;
using TransportTime.Business;
using System.Linq;
using System.Collections.Generic;
using PrimService.Models;
using System.IO;

namespace TransportTime.Notifications
{
    public class AppNotificationManager
    {
        private Context Context { get; }
        private NotificationPusher NotificationPusher { get; }
        public AppNotificationManager(Context context)
        {
            Context = context;
            NotificationPusher = new NotificationPusher(context);
        }

        public void ShowTimeTableNotification(TransportStopData transportStopData)
        {
            ShowLoadingTimeTableNotification(transportStopData);
            UpdateTimeTableNotificationAsync(transportStopData);
        }

        private void ShowLoadingTimeTableNotification(TransportStopData transportStopData)
        {
            var notificationData = new NotificationData(transportStopData);
            var notificationBuilder = new LoadingTimeTableNotificationBuilder(this.Context);
            var notificationWrapper = notificationBuilder.BuildNotification(NotificationPusher.ChannelId, notificationData);
            NotificationPusher.PushNotification(notificationWrapper);
        }
        private void UpdateTimeTableNotificationAsync(TransportStopData transportStopData)
        {
            Task.Run(async () =>
            {
                try
                {
                    NextPassageLite[] timeTable;
                    if (Xamarin.Essentials.Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet)
                        timeTable = await GetTimeTable(transportStopData);
                    else
                        timeTable = null;

                    NotificationBuilder notificationBuilder;
                    if (transportStopData.Type == Consts.TrainRer)
                        notificationBuilder = new TrainTimeTableNotificationBuilder(this.Context);
                    else
                        notificationBuilder = new RatpTimeTableNotificationBuilder(this.Context);

                    var notificationWrapper = notificationBuilder.BuildNotification(NotificationPusher.ChannelId, new NotificationData(transportStopData, timeTable));
                    NotificationPusher.PushNotification(notificationWrapper);
                }
                catch (Exception ex)
                {
                    var errorNotificationBuilder = new ErrorNotificationBuilder(this.Context);

                    var notificationWrapper = errorNotificationBuilder.BuildNotification(NotificationPusher.ChannelId, new NotificationData(transportStopData));
                    NotificationPusher.PushNotification(notificationWrapper);
                }             
            });
           
        }

        private async Task<NextPassageLite[]> GetTimeTable(TransportStopData transportStopData)
        {
            var nextPassages = !string.IsNullOrEmpty(transportStopData.DestinationStopAreaId) 
                ? await Business.PrimService.Instance.GetTimeAsync(transportStopData.StopAreaId, transportStopData.DestinationStopAreaId, transportStopData.LineId) 
                : await Business.PrimService.Instance.GetTimeAsync(transportStopData.StopAreaId, transportStopData.LineId);

            var timeTable = nextPassages.Select(p => new NextPassageLite(p)).ToArray();

            return timeTable.Length > 4 ? TruncateTimeTable(timeTable) : timeTable;
        }

        private NextPassageLite[] TruncateTimeTable(NextPassageLite[] timeTable)
        {
            var truncatedTimeTable = new List<NextPassageLite>();

            foreach (var timeTableElem in timeTable)
            {
                if (truncatedTimeTable.Count(t => !string.IsNullOrEmpty(timeTableElem.DirectionName) && t.DirectionName == timeTableElem.DirectionName) < 2)
                    truncatedTimeTable.Add(timeTableElem);
            }

            return truncatedTimeTable.ToArray();
        }

    }
}