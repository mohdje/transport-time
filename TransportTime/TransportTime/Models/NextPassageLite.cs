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
    public class NextPassageLite
    {
        public string RemainingTime { get; }
        public string CodeMission { get; }
        public string DirectionName { get; }

        public NextPassageLite(NextPassage nextPassage)
        {
            RemainingTime = nextPassage.RemainingTime;
            CodeMission = nextPassage.MissionName;
            DirectionName = nextPassage.DirectionName;
        }
    }
}