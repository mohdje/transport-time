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

namespace TransportTime.Extensions
{
    public static class StringExtensions
    {
        public static string UpperFirstLetter(this string text)
        {
            if (text.Length == 1)
                return char.ToUpper(text[0]).ToString();
            else
                return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}