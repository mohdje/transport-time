using System;
using TransportTime.Models;
using PrimService.Models;
using System.Threading.Tasks;

namespace TransportTime.Notifications
{
    public class ScheduledNotification
    {
        public TransportStopData TransportStopData { get; set; }
        public DayTime ScheduledTime { get; set; }

        public readonly TransportLine Line;

        public ScheduledNotification()
        {

        }

        public ScheduledNotification(ScheduledNotification scheduledNotification)
        {
            TransportStopData = scheduledNotification.TransportStopData;
            ScheduledTime = scheduledNotification.ScheduledTime;
            Line = Business.PrimService.Instance.GetTransportLine(TransportStopData.LineId);
        }

        public int GetId()
        {
            var uniqueString = TransportStopData.Type + TransportStopData.LineId + TransportStopData.StopAreaId + TransportStopData.DestinationStopAreaId + ScheduledTime.Hours + ScheduledTime.Minutes;

            int result = 0;
            foreach (char c in uniqueString)           
                result += System.Convert.ToInt32(c);

            return result;
        }
    }

    public class DayTime
    {       
        public DayOfWeek Day { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
    }
}