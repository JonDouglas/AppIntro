using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AppIntro
{
    public class AppIntro2 : AppIntroBase
    {
        //private static final String TAG = LogHelper.makeLogTag(AppIntro2.class);

        protected View customBackgroundView;
        protected FrameLayout backgroundFrame;
        private List<int> transitionColors;

        protected override int LayoutId => Resource.Layout.intro_layout2;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            backgroundFrame = (FrameLayout) FindViewById(Resource.Id.background);
        }

        public void ShowDoneButton(bool showDone)
        {
            SetProgressButtonEnabled(showDone);
        }

        public void SetImageSkipButton(Drawable imageSkipButton)
        {
            ImageButton nextButton = (ImageButton) FindViewById(Resource.Id.skip);
            nextButton.SetImageDrawable(imageSkipButton);
        }

        public void SetBackgroundView(View view)
        {
            customBackgroundView = view;
            if (customBackgroundView != null)
            {
                backgroundFrame.AddView(customBackgroundView);
            }
        }

        public void SetAnimationColors(List<int> colors)
        {
            transitionColors = colors;
        }
    }
}