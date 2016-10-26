using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using AppIntro.Utils;

namespace AppIntro
{
    public abstract class AppIntro : AppIntroBase
    {
        //private static final String TAG = LogHelper.makeLogTag(AppIntro.class);

        protected override int LayoutId => Resource.Layout.intro_layout;

        public void SetBarColor(int color)
        {
            LinearLayout bottomBar = (LinearLayout) FindViewById(Resource.Id.bottom);
            bottomBar.SetBackgroundColor(new Color(color));
        }

        public void SetNextArrowColor(int color)
        {
            ImageButton nextButton = (ImageButton) FindViewById(Resource.Id.next);
            nextButton.SetColorFilter(new Color(color));
        }

        public void SetSeparatorColor(int color)
        {
            TextView separator = (TextView) FindViewById(Resource.Id.bottom_separator);
            separator.SetBackgroundColor(new Color(color));
        }

        public void SetSkipText(string text)
        {
            TextView skipText = (TextView) FindViewById(Resource.Id.skip);
            skipText.Text = text;
        }

        public void SetSkipTextTypeface(string typeURL)
        {
            TextView skipText = (TextView) FindViewById(Resource.Id.skip);
            if (CustomFontCache.Get(typeURL, this) != null)
            {
                skipText.Typeface = CustomFontCache.Get(typeURL, this);
            }
        }

        public void SetDoneText(string text)
        {
            TextView doneText = (TextView) FindViewById(Resource.Id.done);
            doneText.Text = text;
        }

        public void SetDoneTextTypeface(string typeURL)
        {
            TextView doneText = (TextView) FindViewById(Resource.Id.done);
            if (CustomFontCache.Get(typeURL, this) != null)
            {
                doneText.Typeface = CustomFontCache.Get(typeURL, this);
            }
        }

        public void SetColorDoneText(int colorDoneText)
        {
            TextView doneText = (TextView) FindViewById(Resource.Id.done);
            doneText.SetTextColor(new Color(colorDoneText));
        }

        public void SetColorSkipButton(int colorSkipButton)
        {
            TextView skip = (TextView) FindViewById(Resource.Id.skip);
            skip.SetTextColor(new Color(colorSkipButton));
        }

        public void SetImageNextButton(Drawable imageNextButton)
        {
            ImageView nextButton = (ImageView) FindViewById(Resource.Id.next);
            nextButton.SetImageDrawable(imageNextButton);
        }

        public void ShowDoneButton(bool showDone)
        {
            SetProgressButtonEnabled(showDone);
        }
    }
}
