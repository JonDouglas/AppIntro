using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AppIntro;
using Fragment = Android.Support.V4.App.Fragment;

namespace Sample
{
    [Activity(Theme = "@style/FullscreenTheme")]
    public class CustomIntro : BaseIntro
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AddSlide(AppIntroFragment.NewInstance("Title here", "Description here...\nYeah, I've added this fragment programmatically", Resource.Drawable.ic_slide1, Color.ParseColor("#2196F3")));

            AddSlide(AppIntroFragment.NewInstance("HTML Description", "<b>Description bold...</b><br><i>Description italic...</i>", Resource.Drawable.ic_slide1, Color.ParseColor("#2196F3")));

            SetBarColor(Color.ParseColor("#3F51B5"));
            SetSeparatorColor(Color.ParseColor("#2196F3"));
            ShowSkipButton(false);

            SetVibrate(true);
            SetVibrateIntensity(30);
        }

        public void GetStarted(View v)
        {
            LoadMainActivity();
        }

        public override void OnDonePressed()
        {
            base.OnDonePressed();

            LoadMainActivity();
        }

        public override void OnSkipPressed(Fragment currentFragment)
        {
            base.OnSkipPressed();
            LoadMainActivity();
            Toast.MakeText(ApplicationContext, Resource.String.skip, ToastLength.Short).Show();
        }
    }
}