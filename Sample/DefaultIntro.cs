using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;

namespace Sample
{
    [Activity(Theme = "@style/FullscreenTheme")]
    public class DefaultIntro : BaseIntro
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AddSlide(SampleSlide.NewInstance(Resource.Layout.intro));
            AddSlide(SampleSlide.NewInstance(Resource.Layout.intro2));
            AddSlide(SampleSlide.NewInstance(Resource.Layout.intro3));
            AddSlide(SampleSlide.NewInstance(Resource.Layout.intro4));
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