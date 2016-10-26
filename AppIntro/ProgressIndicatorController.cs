using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using AppIntro.Utils;

namespace AppIntro
{
    public class ProgressIndicatorController : IndicatorController
    {
        public static int DEFAULT_COLOR = 1;
        private static int FIRST_PAGE_NUM = 0;
        private int selectedDotColor = DEFAULT_COLOR;
        private int unselectedDotColor = DEFAULT_COLOR;
        private ProgressBar _progressBar;
        public View NewInstance(Context context)
        {
            _progressBar = (ProgressBar) View.Inflate(context, Resource.Layout.progress_indicator, null);
            if (selectedDotColor != DEFAULT_COLOR)
            {
                _progressBar.ProgressDrawable.SetColorFilter(ColorHelper.FromInt(selectedDotColor), PorterDuff.Mode.SrcIn);
            }
            if (unselectedDotColor != DEFAULT_COLOR)
            {
                _progressBar.IndeterminateDrawable.SetColorFilter(ColorHelper.FromInt(unselectedDotColor), PorterDuff.Mode.SrcIn);
            }
            return _progressBar;
        }

        public void Initialize(int slideCount)
        {
            _progressBar.Max = slideCount;
            SelectPosition(FIRST_PAGE_NUM);
        }

        public void SelectPosition(int index)
        {
            _progressBar.Progress = index + 1;
        }

        public void SetSelectedIndicatorColor(int color)
        {
            this.selectedDotColor = color;
            if (_progressBar != null)
            {
                _progressBar.ProgressDrawable.SetColorFilter(ColorHelper.FromInt(color), PorterDuff.Mode.SrcIn);
            }
        }

        public void SetUnselectedIndicatorColor(int color)
        {
            this.unselectedDotColor = color;
            if (_progressBar != null)
            {
                _progressBar.IndeterminateDrawable.SetColorFilter(ColorHelper.FromInt(color), PorterDuff.Mode.SrcIn);
            }
        }
    }
}