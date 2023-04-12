import FakeData from "@/js/phoneInterface/fakeData.js";


const fakeAsyncCallMobile = (callback, result, delayMillis) => {
	setTimeout(() => {
		callback.call(window, { result: result });
	}, delayMillis);
}

const FakePhoneInterface = {
	searchStopsAreasAsync(text, callback) {
		fakeAsyncCallMobile(
			callback, {
			searchText: text,
			list: FakeData.stopAreas.filter(stopArea => stopArea.name.toLocaleLowerCase().includes(text.toLowerCase()))
		},
			1500);
	},
	getNextPassagesAsync(stopAreaId, callback) {
		fakeAsyncCallMobile(
			callback,
			FakeData.nextPassages,
			2000);
	},
	getNextPassagesForDestinationAsync(stopAreaId, destinationStopAreaId, lineId, callback) {
		fakeAsyncCallMobile(
			callback,
			FakeData.nextPassages2,
			2000);
	},
	getNextPassagesForFavoriteAsync(favoriteId, callback) {
		const favorite = FakeData.favorites.find(n => n.id === favoriteId);

		let nextPassages;
		if (favorite.transportStopData.destinationStopAreaId)
			nextPassages = FakeData.nextPassages2.filter(n => n.line.id === favorite.line.id);
		else
			nextPassages = FakeData.nextPassages.filter(n => n.line.id === favorite.line.id);

		fakeAsyncCallMobile(
			callback,
			nextPassages,
			2000);
	},
	getInfoTrafficAsync(lineId, callback) {
		fakeAsyncCallMobile(
			callback,
			FakeData.infoTraffic,
			1500);
	},
	addFavorite(stopInfos, groupName) {
		console.log(stopInfos);
		let exists = FakeData.favorites.find(f => f.lineId === stopInfos.line.id
			&& f.stopAreaId === stopInfos.stopArea.id
			&& f.destinationStopAreaId === (stopInfos.stopAreaDestination ? stopInfos.stopAreaDestination.id : ''));
		if (exists)
			console.log("Déjà dans la liste des favoris");
		else {
			FakeData.favorites.push({
				transportStopData: {
					stopAreaId: stopInfos.stopArea.id,
					stopAreaName: stopInfos.stopArea.name,
					destinationStopAreaId: stopInfos.stopAreaDestination ? stopInfos.stopAreaDestination.id : '',
					destinationStopAreaName: stopInfos.stopAreaDestination ? stopInfos.stopAreaDestination.name : '',
				},
				line: stopInfos.line,
				groupName: groupName
			});
			console.log("Ajouté aux favoris");
		}
	},
	getFavorites(callback) {
		fakeAsyncCallMobile(
			callback,
			FakeData.favorites,
			1500);
	},
	getFavoritesGroupNames() {
		return [...new Set(FakeData.favorites.map(favorite => favorite.groupName))];
	},
	updateFavoritesList(favoritesList, withError) {
		if (!withError) {
			console.log("Liste de favoris mise à jour");
			FakeData.favorites = favoritesList;
			return true;
		}
		else {
			console.log("Une erreur s'est produite");
			return false;
		}

	},
	pinToNotifications(stopInfos) {
		var desinationName = stopInfos.stopAreaDestination ? stopInfos.stopAreaDestination.name : null;
		console.log(stopInfos.line.name + " " + stopInfos.stopArea.name + " " + desinationName + " ajouté aux notifications");
	},
	scheduleNotification(stopInfos, daysList, hours, minutes) {
		console.log(stopInfos);
		console.log(stopInfos.line.name + " " + stopInfos.stopArea.name + " planifier pour " + daysList + " " + hours + ":" + minutes);
	},
	getScheduledNotifications(callback) {
		console.log("getScheduledNotifications")
		fakeAsyncCallMobile(
			callback,
			FakeData.scheduledNotifications,
			2000);
	},
	deleteScheduledNotifications(scheduledNotification) {
		console.log("deleteScheduledNotifications", scheduledNotification)
		FakeData.scheduledNotifications = FakeData.scheduledNotifications
			.filter(s =>
				!(s.transportStopData.stopId === scheduledNotification.transportStopData.stopId
					&& s.transportStopData.destinationId === scheduledNotification.transportStopData.destinationId
					&& s.line.id === scheduledNotification.line.id
					&& s.scheduledTime.day === scheduledNotification.scheduledTime.day
					&& s.scheduledTime.hours === scheduledNotification.scheduledTime.hours
					&& s.scheduledTime.minutes === scheduledNotification.scheduledTime.minutes));
	},
	openAppSettings() {
		console.log("Open app settings");
	},
}

export default FakePhoneInterface;