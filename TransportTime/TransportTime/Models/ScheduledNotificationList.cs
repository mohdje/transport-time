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
using TransportTime.Notifications;
using XamarinCustomHelper.IO;
using XamarinCustomHelper.IO.Serialization;

namespace TransportTime.Models
{
    public class ScheduledNotificationList : SavableList<ScheduledNotification>
    {
        private int _maxElements = 30;

        public ScheduledNotificationList(ISerializer serializer) : base(serializer)
        {

        }
        protected override bool CheckBeforeAddElement(ScheduledNotification element, out string errorMessage)
        {
            if (_list.Count == _maxElements)
            {
                errorMessage = "Vous ne pouvez pas planifier plus de " + _maxElements + " affichages d'horaires";
                return false;
            }

            if (_list.Any(s => AreEquals(s, element)))
            {
                errorMessage = string.Empty;
                return false;
            }

            errorMessage = string.Empty;

            return true;
        }

        protected override bool CheckBeforeAddRange(ScheduledNotification[] elements, out string errorMessage)
        {
            if (_list.Count + elements.Length > _maxElements)
            {
                errorMessage = "Vous ne pouvez pas planifier plus de " + _maxElements + " affichages d'horaires";
                return false;
            }

            foreach (var element in elements)
            {
                if (_list.Any(s => AreEquals(s, element)))
                {
                    errorMessage = BuildErrorMessage(element);
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }

        protected override string GetSerializationFileName()
        {
            return "ScheduledPin_v3.xml";
        }

        private bool AreEquals(ScheduledNotification element1, ScheduledNotification element2)
        {
            return element1.TransportStopData.LineId == element2.TransportStopData.LineId
            && element1.TransportStopData.StopAreaId == element2.TransportStopData.StopAreaId
            && element1.TransportStopData.DestinationStopAreaId == element2.TransportStopData.DestinationStopAreaId
            && element1.ScheduledTime.Day == element2.ScheduledTime.Day
            && element1.ScheduledTime.Hours == element2.ScheduledTime.Hours
            && element1.ScheduledTime.Minutes == element2.ScheduledTime.Minutes;
        }

        private string BuildErrorMessage(ScheduledNotification scheduledTransportTimeNotification)
        {
            var message = $"L'affichage d'horaires planifié pour la ligne {scheduledTransportTimeNotification.TransportStopData.LineName}, {scheduledTransportTimeNotification.TransportStopData.StopAreaName}";

            if (!string.IsNullOrEmpty(scheduledTransportTimeNotification.TransportStopData.DestinationStopAreaId))
                message += $", à destination de {scheduledTransportTimeNotification.TransportStopData.DestinationStopAreaName}";

            message += " existe déjà";

            return message;
        }
    }
}