using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TransportTime.Models
{
    public class TransportStopData
    {
        public string Type { get; set; }
        public string LineId { get; set; }
        public string LineName { get; set; }
        public string StopAreaId { get; set; }
        public string StopAreaName { get; set; }
        public string DestinationStopAreaName { get; set; }
        public string DestinationStopAreaId { get; set; }

        public int Hash => GetHash();

        public TransportStopData()
        {

        }

        public TransportStopData(string type, string lineId, string lineName, string stopAreaId, string stopAreaName, string destinationStopAreaId, string destinationStopAreaName)
        {
            Type = type;
            LineId = lineId;
            LineName = lineName;
            StopAreaId = stopAreaId;
            StopAreaName = stopAreaName;
            DestinationStopAreaId = destinationStopAreaId;
            DestinationStopAreaName = destinationStopAreaName;
        }

        private int GetHash()
        {
            var uniqueString = Type + LineId + StopAreaId + DestinationStopAreaId;
            int result = 0;
            foreach (char c in uniqueString)
                result += System.Convert.ToInt32(c);
            return result;
        }
    }
}