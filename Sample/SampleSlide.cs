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
    public class SampleSlide : Fragment
    {
        private static string ARG_LAYOUT_RES_ID = "layoutResId";
        private int layoutResId;

        public static SampleSlide NewInstance(int layoutResId)
        {
            SampleSlide sampleSlide = new SampleSlide();
            Bundle args = new Bundle();
            args.PutInt(ARG_LAYOUT_RES_ID, layoutResId);
            sampleSlide.Arguments = args;
            return sampleSlide;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Arguments != null && Arguments.ContainsKey(ARG_LAYOUT_RES_ID))
            {
                layoutResId = Arguments.GetInt(ARG_LAYOUT_RES_ID);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(layoutResId, container, false);
        }
    }
}