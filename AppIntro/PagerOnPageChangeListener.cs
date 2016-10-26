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
    public class PagerOnPageChangeListener : Java.Lang.Object, ViewPager.IOnPageChangeListener
    {
        public void OnPageScrollStateChanged(int state)
        {
            throw new NotImplementedException();
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            throw new NotImplementedException();
        }

        public void OnPageSelected(int position)
        {
            throw new NotImplementedException();
        }
    }
}