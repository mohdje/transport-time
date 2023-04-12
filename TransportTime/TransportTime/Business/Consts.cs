using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TransportTime.Business
{
    public static class Consts
    {
        public const string TrainRer = "train/rer";
        public const string Metro = "metro";
        public const string Tram = "tram";
        public const string Bus = "bus";
        public const string Bis = "bis";
        public static string DatabaseFolderPath => Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "lucene_database");

    }
}