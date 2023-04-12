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
using XamarinCustomHelper.IO;
using XamarinCustomHelper.IO.Serialization;

namespace TransportTime.Models
{
    public class FavoritesList : SavableList<Favorite>
    {
        public FavoritesList(ISerializer serializer) : base(serializer)
        {

        }   
        protected override bool CheckBeforeAddElement(Favorite element, out string errorMessage)
        {
            var maxFavorites = 30;
            if (_list.Count == maxFavorites)
            {
                errorMessage = $"Vous ne pouvez pas avoir plus de {maxFavorites} favoris";
                return false;
            }

            if (_list.Any(f => AreEquals(f, element)))
            {
                errorMessage = $"Déjà présent dans le groupe {element.GroupName}";
                return false;
            }

            errorMessage = string.Empty;

            return true;
        }

        protected override bool CheckBeforeAddRange(Favorite[] elements, out string errorMessage)
        {
            foreach (var element in elements)
            {
                if (!CheckBeforeAddElement(element, out errorMessage))
                    return false;
            }

            errorMessage = "";
            return true;
        }

        protected override string GetSerializationFileName()
        {
            return "Favorites_v3.xml";
        }

        private bool AreEquals(Favorite element1, Favorite element2)
        {
            return element1.TransportStopData.LineId == element2.TransportStopData.LineId
            && element1.TransportStopData.StopAreaId == element2.TransportStopData.StopAreaId
            && element1.TransportStopData.DestinationStopAreaId == element2.TransportStopData.DestinationStopAreaId
            && element1.GroupName == element2.GroupName;
        }
    }
}