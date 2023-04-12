using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PrimService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransportTime.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public int IndexInGroup { get; set; }
        public TransportStopData TransportStopData { get; set; }

        public readonly TransportLine Line;

        public Favorite()
        {

        }
        public Favorite(Favorite favorite)
        {
            Id = favorite.Id;
            GroupName = favorite.GroupName;
            IndexInGroup = favorite.IndexInGroup;
            TransportStopData = favorite.TransportStopData;
            Line = Business.PrimService.Instance.GetTransportLine(TransportStopData.LineId);
        }
    }
}