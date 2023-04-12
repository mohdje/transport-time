using Android.Content;
using TransportTime.Models;
using TransportTime.Notifications;

namespace TransportTime.BroadcastReceivers
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class ShowTransportTimeNotificationReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var transportStopData = new TransportStopData()
            {
                Type = intent.GetStringExtra("transportType"),
                LineId = intent.GetStringExtra("lineId"),
                LineName = intent.GetStringExtra("lineName"),
                StopAreaId = intent.GetStringExtra("stopAreaId"),
                StopAreaName = intent.GetStringExtra("stopAreaName"),
                DestinationStopAreaId = intent.GetStringExtra("destinationStopAreaId"),
                DestinationStopAreaName = intent.GetStringExtra("destinationStopAreaName")
            };

            var notificationManager = new AppNotificationManager(context);
            notificationManager.ShowTimeTableNotification(transportStopData);
        }
    }
}