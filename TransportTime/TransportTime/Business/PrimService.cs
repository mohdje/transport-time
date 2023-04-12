using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PrimService.Models;
using PrimService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportTime.Business
{
    internal class PrimService
    {
        private static PrimService _instance;

        public static PrimService Instance 
        { 
            get 
            { 
                if(_instance == null)
                    _instance = new PrimService();

                return _instance; 
            } 
        }

        private PrimDbSearcher primDbSearcher;
        private PrimRequester primRequester;
   
        private PrimService()
        {
            primRequester = new PrimRequester(new HttpClientFactory(), new PrimTokenManager(Token.Values));
            PrimDbSearcher.Init(Consts.DatabaseFolderPath);
            primDbSearcher = new PrimDbSearcher();
        }

        /// <summary>
        /// Instantiate a static instance of PrimService
        /// </summary>
        public static void Init()
        {
            _instance = new PrimService();
        }

        public IEnumerable<StopArea> SearchStopAreas(string text)
        {
            return primDbSearcher.SearchStopAreas(text, 100);
        }

        public async Task<IEnumerable<NextPassage>> GetTimeAsync(string stopAreaId)
        {
            var result = await primRequester.GetTimeAsync(stopAreaId);
            return FilterNextPassages(result);
        }

        public async Task<IEnumerable<NextPassage>> GetTimeAsync(string stopAreaId, string lineId)
        {
            var result = await primRequester.GetTimeAsync(stopAreaId, lineId);
            return FilterNextPassages(result);
        }

        public async Task<IEnumerable<NextPassage>> GetTimeAsync(string stopAreaId, string destinationStopAreaId, string lineId)
        {
            var nextPassages = await GetTimeAsync(stopAreaId, lineId);
            var destinationNextPassages = await GetTimeAsync(destinationStopAreaId, lineId);
            return KeepNextPassagesForItinerary(nextPassages, destinationNextPassages);
        }

        public async Task<InfoTraffic> GetInfoTrafficAsync(string lineId)
        {
            return await primRequester.GetInfoTrafficAsync(lineId);
        }

        public TransportLine GetTransportLine(string lineId)
        {
            return primDbSearcher.GetTransportLine(lineId);
        }

        private IEnumerable<NextPassage> FilterNextPassages(IEnumerable<NextPassage> nextPassages)
        {
            return nextPassages.Where(p => !string.IsNullOrEmpty(p.RemainingTime)).OrderBy(p => p.DepartureTime);
        }

        public NextPassage[] KeepNextPassagesForItinerary(IEnumerable<NextPassage> nextPassagesAtStartPoint, IEnumerable<NextPassage> nextPassagesAtDestinationPoint)
        {
            var nextPassagesToKeep = new List<NextPassage>();

            foreach (var npStart in nextPassagesAtStartPoint)
            {
                if (string.IsNullOrEmpty(npStart.TrainId) && string.IsNullOrEmpty(npStart.MissionName))
                    continue;

                var nextPassagesSameTrainId = nextPassagesAtDestinationPoint.SingleOrDefault(npDest => npDest.TrainId == npStart.TrainId && npDest.DepartureTime > npStart.DepartureTime);
                if (nextPassagesSameTrainId != null)
                {
                    npStart.SetDestinationArrivalTime(nextPassagesSameTrainId.DepartureTime);
                    nextPassagesToKeep.Add(npStart);
                }
                else
                {
                    var nextPassagesSameDirection = nextPassagesAtDestinationPoint.Where(
                        npDest => npDest.MissionName == npStart.MissionName 
                        && HaveSameDirection(npStart.StopPoint, npDest.StopPoint, npStart.DestinationStopPoint));
                    if (nextPassagesSameDirection.Any())
                        nextPassagesToKeep.Add(npStart);
                }
            }
            
            return nextPassagesToKeep.Take(4).ToArray();
        }

        private bool HaveSameDirection(StopPoint stopPointStart, StopPoint stopPointDestination, StopPoint stopPointTerminus)
        {
            var vector1 = new
            {
                X = float.Parse(stopPointDestination.PositionX) - float.Parse(stopPointStart.PositionX),
                Y = float.Parse(stopPointDestination.PositionY) - float.Parse(stopPointStart.PositionY)
            };

            var vector2 = new
            {
                X = float.Parse(stopPointTerminus.PositionX) - float.Parse(stopPointStart.PositionX),
                Y = float.Parse(stopPointTerminus.PositionY) - float.Parse(stopPointStart.PositionY)
            };

            var scalar = (vector1.X * vector2.X) + (vector1.Y * vector2.Y);

            return scalar > 0;
        }
    }
}