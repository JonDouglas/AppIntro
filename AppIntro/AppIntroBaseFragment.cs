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
using AppIntro.Utils;
using Fragment = Android.Support.V4.App.Fragment;

namespace AppIntro
{
    public abstract class AppIntroBaseFragment : Fragment, ISlideSelectionListener, ISlideBackgroundColorHolder
    {
        protected static string ARG_TITLE = "title";
        protected static string ARG_TITLE_TYPEFACE = "title_typeface";
        protected static string ARG_DESC = "desc";
        protected static string ARG_DESC_TYPEFACE = "desc_typeface";
        protected static string ARG_DRAWABLE = "drawable";
        protected static string ARG_BG_COLOR = "bg_color";
        protected static string ARG_TITLE_COLOR = "title_color";
        protected static string ARG_DESC_COLOR = "desc_color";
        //private static string TAG = LogHelper.makeLogTag(AppIntroBaseFragment.class);
        private int drawable, bgColor, titleColor, descColor, layoutId;
        private string title, titleTypeface, description, descTypeface;
        private LinearLayout mainLayout;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RetainInstance = true;

            if (Arguments != null && Arguments.Size() != 0)
            {
                drawable = Arguments.GetInt(ARG_DRAWABLE);
                title = Arguments.GetString(ARG_TITLE);
                titleTypeface = Arguments.ContainsKey(ARG_TITLE_TYPEFACE) ?
                        Arguments.GetString(ARG_TITLE_TYPEFACE) : "";
                description = Arguments.GetString(ARG_DESC);
                descTypeface = Arguments.ContainsKey(ARG_DESC_TYPEFACE) ?
                        Arguments.GetString(ARG_DESC_TYPEFACE) : "";
                bgColor = Arguments.GetInt(ARG_BG_COLOR);
                titleColor = Arguments.ContainsKey(ARG_TITLE_COLOR) ?
                        Arguments.GetInt(ARG_TITLE_COLOR) : 0;
                descColor = Arguments.ContainsKey(ARG_DESC_COLOR) ?
                        Arguments.GetInt(ARG_DESC_COLOR) : 0;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(LayoutId, container, false);
            TextView t = (TextView)v.FindViewById(Resource.Id.title);
            TextView d = (TextView) v.FindViewById(Resource.Id.description);
            ImageView i = (ImageView) v.FindViewById(Resource.Id.image);
            mainLayout = (LinearLayout) v.FindViewById(Resource.Id.main);

            t.Text = title;
            if (titleColor != 0)
            {
                t.SetTextColor(new Color(titleColor));
            }
            if (titleTypeface != null)
            {
                if (CustomFontCache.Get(titleTypeface, Context) != null)
                {
                    t.Typeface = CustomFontCache.Get(titleTypeface, Context);
                }
            }
            d.Text = description;
            if (descColor != 0)
            {
                d.SetTextColor(new Color(descColor));
            }
            if (descTypeface != null)
            {
                if (CustomFontCache.Get(descTypeface, Context) != null)
                {
                    d.Typeface = CustomFontCache.Get(descTypeface, Context);
                }
            }
            i.SetImageResource(drawable);
            mainLayout.SetBackgroundColor(new Color(bgColor));

            return v;
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt(ARG_DRAWABLE, drawable);
            outState.PutString(ARG_TITLE, title);
            outState.PutString(ARG_DESC, description);
            outState.PutInt(ARG_BG_COLOR, bgColor);
            outState.PutInt(ARG_TITLE_COLOR, titleColor);
            outState.PutInt(ARG_DESC_COLOR, descColor);
            base.OnSaveInstanceState(outState);
        }

        public void OnSlideSelected()
        {
            //LogHelper.d(TAG, String.format("Slide %s has been selected.", title));
        }

        public void OnSlideDeselected()
        {
            //LogHelper.d(TAG, String.format("Slide %s has been deselected.", title));
        }

        protected abstract int LayoutId { get; }
        public int GetDefaultBackgroundColor()
        {
            return bgColor;
        }

        public void SetBackgroundColor(int backgroundColor)
        {
            mainLayout.SetBackgroundColor(new Color(backgroundColor));
        }
    }
}