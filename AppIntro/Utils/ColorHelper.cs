using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AppIntro.Utils
{
    public class ColorHelper
    {
        public static Color FromInt(int current)
        {
            if (current != -1)
            {
                var currentString = current.ToString();
                string AA = currentString.Substring(0, 2);
                string RR = currentString.Substring(2, 4);
                string GG = currentString.Substring(4, 6);
                string BB = currentString.Substring(6, 8);
                int[] argbInts = new[] { int.Parse(AA), int.Parse(RR), int.Parse(GG), int.Parse(BB) };

                return Color.Argb(argbInts[0], argbInts[1], argbInts[2], argbInts[3]);
            }
            else
            {
                return new Color();
            }
        }
    }
}