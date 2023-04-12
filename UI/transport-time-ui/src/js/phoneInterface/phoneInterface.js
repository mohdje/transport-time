import FakePhoneInterface from "@/js/phoneInterface/fakePhoneInterface.js";

/* global TransportTimeApp*/
/* global Phone*/

const ProdPhoneInterface = {
    searchStopsAreasAsync(text, callback){
      TransportTimeApp.SearchStopsAreasAsync(text, callback.toString());
    },
    getNextPassagesAsync(stopAreaId, callback){
      TransportTimeApp.GetNextPassagesAsync(stopAreaId, callback.toString());
    },
    getNextPassagesForDestinationAsync(stopAreaId, destinationStopAreaId, lineId, callback){
      TransportTimeApp.GetNextPassagesForDestinationAsync(stopAreaId, destinationStopAreaId, lineId, callback.toString());
    },
    getNextPassagesForFavoriteAsync(favoriteId, callback){
      TransportTimeApp.GetNextPassagesForFavoriteAsync(favoriteId, callback.toString());
    },
    getInfoTrafficAsync(lineId, callback){
      TransportTimeApp.GetInfoTrafficAsync(lineId, callback.toString());
    },
    getFavorites(callback){
      TransportTimeApp.GetFavoritesAsync(callback.toString());
    },
    getFavoritesGroupNames(){
      return JSON.parse(TransportTimeApp.GetFavoriteGroups());
   },
    addFavorite(stopInfos, groupName){
      const destinationStopAreaId = stopInfos.stopAreaDestination ? stopInfos.stopAreaDestination.id : null;
      const destinationStopAreaName = stopInfos.stopAreaDestination ? stopInfos.stopAreaDestination.name : null;
      TransportTimeApp.AddFavorite(stopInfos.line.type, stopInfos.line.id, stopInfos.line.name, stopInfos.stopArea.id, stopInfos.stopArea.name, destinationStopAreaId, destinationStopAreaName, groupName);
    },
    updateFavoritesList(favoritesList){
      TransportTimeApp.UpdateFavoritesList(JSON.stringify(favoritesList));
    },
    pinToNotifications(stopInfos){
      const destinationStopAreaId = stopInfos.stopAreaDestination ? stopInfos.stopAreaDestination.id : null;
      const destinationStopAreaName = stopInfos.stopAreaDestination ? stopInfos.stopAreaDestination.name : null;
      TransportTimeApp.PinToNotifications(stopInfos.line.type, stopInfos.line.id, stopInfos.line.name, stopInfos.stopArea.id, stopInfos.stopArea.name, destinationStopAreaId, destinationStopAreaName);
    },
    scheduleNotification(stopInfos, daysList, hours, minutes){
      const destinationStopAreaId = stopInfos.stopAreaDestination ? stopInfos.stopAreaDestination.id : null;
      const destinationStopAreaName = stopInfos.stopAreaDestination ? stopInfos.stopAreaDestination.name : null;
      TransportTimeApp.ScheduleNotification(stopInfos.line.type, stopInfos.line.id, stopInfos.line.name, stopInfos.stopArea.id, stopInfos.stopArea.name, destinationStopAreaId, destinationStopAreaName, daysList, hours, minutes);
    },
    getScheduledNotifications(callback){
      TransportTimeApp.GetScheduledNotifications(callback.toString());
    },
    deleteScheduledNotifications(scheduledNotification){
      TransportTimeApp.RemoveScheduledNotification(
        scheduledNotification.line.type, 
        scheduledNotification.line.id, 
        scheduledNotification.line.name, 
        scheduledNotification.transportStopData.stopAreaId, 
        scheduledNotification.transportStopData.stopAreaName, 
        scheduledNotification.transportStopData.destinationStopAreaId, 
        scheduledNotification.transportStopData.destinationStopAreaName, 
        scheduledNotification.scheduledTime.day, 
        scheduledNotification.scheduledTime.hours, 
        scheduledNotification.scheduledTime.minutes);
    },
    openAppSettings(){
      Phone.OpenAppSettings();
    },
}

export const PhoneInterface = process.env.NODE_ENV === "development" ? FakePhoneInterface : ProdPhoneInterface;