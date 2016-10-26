using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;

namespace AppIntro
{
    public class FadePageTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        public void TransformPage(View page, float position)
        {
            page.TranslationX = page.Width*-position;

            if (position <= -1.0F || position >= 1.0F)
            {
                page.Alpha = 0.0F;
                page.Clickable = false;
            }
            else if (position == 0.0F)
            {
                page.Alpha = 1.0F;
                page.Clickable = true;
            }
            else
            {
                page.Alpha = 1.0F - Math.Abs(position);
            }
        }
    }
}