using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Webkit;
using Android.Widget;
using Java.Interop;
using TransportTime.Models;
using XamarinCustomHelper.Javascript;
using XamarinCustomHelper.Activities;
using XamarinCustomHelper.IO;
using TransportTime.Notifications;
using TransportTime.BroadcastReceivers;
using AppNotificationManager = TransportTime.Notifications.AppNotificationManager;
using System.Threading;
using System.Threading.Tasks;
using PrimService.Models;
using System.IO;

namespace TransportTime.Activities
{
    internal class JSIMainActivity : JavascriptWebViewInterface
    {
        JavascriptPhoneInterface jsiPhone;
        FavoritesList favoritesList;
        ScheduledNotificationList scheduledNotificationList;
        AppNotificationManager notificationManager;
        NotificationScheduler notificationScheduler;
        CancellationTokenSource stopAreaCancellationTokenSource;
        public JSIMainActivity(WebViewActivity context) : base(context)
        {
            jsiPhone = new JavascriptPhoneInterface(context);
            favoritesList = new FavoritesList(new AndroidXmlSerializer(context));
            scheduledNotificationList = new ScheduledNotificationList(new AndroidXmlSerializer(context));
            notificationManager = new AppNotificationManager(context);
            notificationScheduler = new NotificationScheduler(context);
        }

        public override string InterfaceName()
        {
            return "TransportTimeApp";
        }

        #region Search

        [Export]
        [JavascriptInterface]
        public void SearchStopsAreasAsync(string text, string callback)
        {
            if(stopAreaCancellationTokenSource != null)
                stopAreaCancellationTokenSource.Cancel();

            stopAreaCancellationTokenSource = new CancellationTokenSource();

            AsyncCall.ExecuteAsync(Context, () =>
            {
                var list = Business.PrimService.Instance.SearchStopAreas(text).ToArray();

                return new
                {
                    List = list,
                    SearchText = text
                };

            }, callback, stopAreaCancellationTokenSource);
        }

        [Export]
        [JavascriptInterface]
        public void GetNextPassagesAsync(string stopAreaId, string callback)
        {
            AsyncCall.ExecuteAsync(Context, async () =>
            {
                if (jsiPhone.CheckConnectivity() == 0)
                {
                    jsiPhone.ShowToastMessage("Vérifiez votre connexion internet");
                    return null;
                }

                var result = await Business.PrimService.Instance.GetTimeAsync(stopAreaId);
                return result?.ToArray();

            }, callback);
        }

        [Export]
        [JavascriptInterface]
        public void GetNextPassagesForDestinationAsync(string stopAreaId, string destinationStopAreaId, string lineId, string callback)
        {
            AsyncCall.ExecuteAsync(Context, async () =>
            {
                if (jsiPhone.CheckConnectivity() == 0)
                {
                    jsiPhone.ShowToastMessage("Vérifiez votre connexion internet");
                    return null;
                }

                var result = await Business.PrimService.Instance.GetTimeAsync(stopAreaId, destinationStopAreaId, lineId);
                return result?.ToArray();

            }, callback);
        }

        [Export]
        [JavascriptInterface]
        public void GetInfoTrafficAsync(string lineId, string callback)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            AsyncCall.ExecuteAsync(Context, () =>
            {
                return Business.PrimService.Instance.GetInfoTrafficAsync(lineId).Result;

            }, callback, cancellationTokenSource);
        }

        #endregion

        #region Favorites

        [Export]
        [JavascriptInterface]
        public void GetFavoritesAsync(string callback)
        {
            AsyncCall.ExecuteAsync(this.Context, () =>
            {
                return favoritesList.Elements.Select(f => new Favorite(f));
            }, callback, new CancellationTokenSource());              
        }

        [Export]
        [JavascriptInterface]
        public string GetFavoriteGroups()
        {
            var groups = new List<string>();
            if (favoritesList.Elements.Any())
                groups.AddRange(favoritesList.Elements.Select(e => e.GroupName).Distinct());

            var defaultGroupLabel = "Autre";
            if (!groups.Contains(defaultGroupLabel))
                groups.Add(defaultGroupLabel);

            return JsonHelper.ToJson(groups);
        }

        [Export]
        [JavascriptInterface]
        public void GetNextPassagesForFavoriteAsync(int favoriteId, string callback)
        {
            AsyncCall.ExecuteAsync(Context, async () =>
            {
                if (jsiPhone.CheckConnectivity() == 0)
                {
                    jsiPhone.ShowToastMessage("Vérifiez votre connexion internet");
                    return null;
                }

                var favorite = favoritesList.Elements.Single(e => e.Id == favoriteId);

                IEnumerable<NextPassage> nextPasages;
                if (!string.IsNullOrEmpty(favorite.TransportStopData.DestinationStopAreaId))
                    nextPasages = await Business.PrimService.Instance.GetTimeAsync(favorite.TransportStopData.StopAreaId, favorite.TransportStopData.DestinationStopAreaId, favorite.TransportStopData.LineId);
                else
                    nextPasages = await Business.PrimService.Instance.GetTimeAsync(favorite.TransportStopData.StopAreaId, favorite.TransportStopData.LineId);

                return nextPasages?.ToArray();

            }, callback);
        }

        [Export]
        [JavascriptInterface]
        public void AddFavorite(string type, string lineId, string lineName, string stopAreaId, string stopAreaName, string destinationStopAreaId, string destinationStopAreaName, string groupName)
        {
            try
            {
                var transportStopData = new TransportStopData(type, lineId, lineName, stopAreaId, stopAreaName, destinationStopAreaId, destinationStopAreaName);
                var indexInGroup = favoritesList.Elements.Any(e => e.GroupName == groupName) ? favoritesList.Elements.Where(e => e.GroupName == groupName).Max(e => e.IndexInGroup) + 1 : 0;
                var newFavoriteId = favoritesList.Elements.Any() ?  favoritesList.Elements.Max(e => e.Id) + 1 : 0;
                favoritesList.Add(new Favorite 
                { 
                    Id = newFavoriteId,
                    GroupName = groupName, 
                    IndexInGroup = indexInGroup, 
                    TransportStopData = transportStopData 
                });
            }
            catch (Exception ex)
            {
                jsiPhone.ShowToastMessage(ex.Message);
                return;
            }

            jsiPhone.ShowToastMessage($"Ajouté au groupe {groupName}");
        }

        [Export]
        [JavascriptInterface]
        public bool UpdateFavoritesList(string jsonFavoritesList)
        {
            var updatedFavoritesList = JsonHelper.ToObject<Favorite[]>(jsonFavoritesList);
            try
            {
                favoritesList.Clear();
                favoritesList.AddRange(updatedFavoritesList);
            }
            catch (Exception ex)
            {
                jsiPhone.ShowToastMessage("Une erreur s'est produite");
                return false;
            }

            return true;
        }


        #endregion

        #region Notifications

        [Export]
        [JavascriptInterface]
        public void PinToNotifications(string type, string lineId, string lineName, string stopAreaId, string stopAreaName, string destinationStopAreaId, string destinationStopAreaName)
        {
            try
            {
                var transportStopData = new TransportStopData(type, lineId, lineName, stopAreaId, stopAreaName, destinationStopAreaId, destinationStopAreaName);
                notificationManager.ShowTimeTableNotification(transportStopData);
            }
            catch (Exception)
            {
                jsiPhone.ShowToastMessage("Une erreur est survenue");
                return;
            }

            jsiPhone.ShowToastMessage("Les horaires sont épinglés à votre écran de notifications");
        }

        [Export]
        [JavascriptInterface]
        public void ScheduleNotification(string type, string lineId, string lineName, string stopAreaId, string stopAreaName, string destinationStopAreaId, string destinationStopAreaName, string daysList, int hours, int minutes)
        {
            try
            {
                var days = daysList.Split(',');

                List<ScheduledNotification> scheduledTransportTimes = new List<ScheduledNotification>();

                foreach (var day in days)
                {
                    var schedulePin = new ScheduledNotification()
                    {
                        TransportStopData = new TransportStopData(type, lineId, lineName, stopAreaId, stopAreaName, destinationStopAreaId, destinationStopAreaName),
                        ScheduledTime = new DayTime()
                        {
                            Day = (DayOfWeek)int.Parse(day),
                            Hours = hours,
                            Minutes = minutes
                        }
                    };
                    scheduledTransportTimes.Add(schedulePin);
                }
                scheduledNotificationList.AddRange(scheduledTransportTimes.ToArray());

                Android.Content.Intent scheduleIntent = new Android.Content.Intent(this.Context, typeof(ScheduleTransportTimeNotificationReceiver));
                Context.SendBroadcast(scheduleIntent);
            }
            catch (Exception ex)
            {
                var errorMessage = string.IsNullOrEmpty(ex.Message) ? "Une erreur est survenue lors de la planification d'affichage d'horaires" : ex.Message;
                jsiPhone.ShowToastMessage(errorMessage);
                return;
            }

            jsiPhone.ShowToastMessage("Affichage d'horaires planifié avec succès");
        }

        [Export]
        [JavascriptInterface]
        public void GetScheduledNotifications(string callback)
        {
            AsyncCall.ExecuteAsync(Context, () =>
            {
                return scheduledNotificationList.Elements.Select(e => new ScheduledNotification(e));

            }, callback, new CancellationTokenSource());
        }

        [Export]
        [JavascriptInterface]
        public void RemoveScheduledNotification(string type, string lineId, string lineName, string stopAreaId, string stopAreaName, string destinationStopAreaId, string destinationStopAreaName, int day, int hours, int minutes)
        {
            try
            {
                scheduledNotificationList.Remove(s =>
                    s.ScheduledTime.Day == (DayOfWeek)day && s.ScheduledTime.Hours == hours && s.ScheduledTime.Minutes == minutes &&
                     s.TransportStopData.LineId == lineId && s.TransportStopData.StopAreaId == stopAreaId && s.TransportStopData.DestinationStopAreaId == destinationStopAreaId);

                notificationScheduler.CancelTransportTimeNotification(new ScheduledNotification()
                {
                    TransportStopData = new TransportStopData(type, lineId, lineName, stopAreaId, stopAreaName, destinationStopAreaId, destinationStopAreaName),
                    ScheduledTime = new DayTime()
                    {
                        Day = (DayOfWeek)day,
                        Hours = hours,
                        Minutes = minutes
                    }
                });

            }
            catch (Exception ex)
            {
                jsiPhone.ShowToastMessage(ex.Message);
                return;
            }

            jsiPhone.ShowToastMessage("L'affichage planifié a été supprimé");
        }

        #endregion
    }
}