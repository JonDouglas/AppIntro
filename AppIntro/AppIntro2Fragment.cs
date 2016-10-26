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

namespace AppIntro
{
    public class AppIntro2Fragment : AppIntroBaseFragment
    {
        public static AppIntro2Fragment NewInstance(string title, string description, int imageDrawable,
            int bgColor)
        {
            return NewInstance(title, description, imageDrawable, bgColor, 0, 0);
        }

        public static AppIntro2Fragment NewInstance(string title, string description, int imageDrawable,
            int bgColor, int titleColor, int descColor)
        {
            AppIntro2Fragment slide = new AppIntro2Fragment();
            Bundle args = new Bundle();
            args.PutString(ARG_TITLE, title);
            args.PutString(ARG_TITLE_TYPEFACE, null);
            args.PutString(ARG_DESC, description);
            args.PutString(ARG_DESC_TYPEFACE, null);
            args.PutInt(ARG_DRAWABLE, imageDrawable);
            args.PutInt(ARG_BG_COLOR, bgColor);
            args.PutInt(ARG_TITLE_COLOR, titleColor);
            args.PutInt(ARG_DESC_COLOR, descColor);
            slide.Arguments = args;
            return slide;
        }

        public static AppIntro2Fragment NewInstance(string title, string titleTypeFace, string description,
            string descTypeface, int imageDrawable,
            int bgColor)
        {
            return NewInstance(title, titleTypeFace, description, descTypeface, imageDrawable, bgColor, 0, 0);
        }

        public static AppIntro2Fragment NewInstance(string title, string titleTypeFace, string description,
            string descTypeface, int imageDrawable,
            int bgColor, int titleColor, int descColor)
        {
            AppIntro2Fragment slide = new AppIntro2Fragment();
            Bundle args = new Bundle();
            args.PutString(ARG_TITLE, title);
            args.PutString(ARG_TITLE_TYPEFACE, titleTypeFace);
            args.PutString(ARG_DESC, description);
            args.PutString(ARG_DESC_TYPEFACE, descTypeface);
            args.PutInt(ARG_DRAWABLE, imageDrawable);
            args.PutInt(ARG_BG_COLOR, bgColor);
            args.PutInt(ARG_TITLE_COLOR, titleColor);
            args.PutInt(ARG_DESC_COLOR, descColor);
            slide.Arguments = args;
            return slide;
        }

        protected override int LayoutId => Resource.Layout.fragment_intro2;
    }
}