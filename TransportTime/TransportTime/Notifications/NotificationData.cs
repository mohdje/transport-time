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
using TransportTime.Extensions;
using TransportTime.Business;

namespace TransportTime.Notifications
{
    public class NotificationData
    {
        public string Title { get; }

        public TransportStopData TransportStopData { get; }

        public string LoadingMessage => "Mise à jour des horaires...";
        public string ExpandMessage => "Glissez vers le bas";
        public string ErrorMessage => "Une erreur est survenue";

        public NextPassageLite[] NextPassages { get;  }

        public NotificationData(TransportStopData transportStopData, NextPassageLite[] nextPassages = null)
        {
            Title = BuildTitle(transportStopData); 
            TransportStopData = transportStopData;
            NextPassages = nextPassages;
        }

        private string BuildTitle(TransportStopData transportStopData)
        {
            var title = $"{GetType(transportStopData)} {transportStopData.LineName}, {transportStopData.StopAreaName}";

            if (!string.IsNullOrEmpty(transportStopData.DestinationStopAreaId))
                title += $", destination {transportStopData.DestinationStopAreaName}";

            return title;
        }

        private string GetType(TransportStopData transportStopData)
        {
            return transportStopData.Type.Equals("rail", StringComparison.OrdinalIgnoreCase) ? "Rer/Train" : transportStopData.Type.UpperFirstLetter();
        }
    }
}