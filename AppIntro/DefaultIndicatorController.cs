using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using AppIntro.Utils;

namespace AppIntro
{
    public class DefaultIndicatorController : IndicatorController
    {
        public static int DEFAULT_COLOR = 1;
        private static int FIRST_PAGE_NUM = 0;
        private int selectedDotColor = DEFAULT_COLOR;
        private int unselectedDotColor = DEFAULT_COLOR;
        private int _currentPosition;
        private Context _context;
        private LinearLayout _dotLayout;
        private List<ImageView> _dots;
        private int _slideCount;
        public View NewInstance(Context context)
        {
            _context = context;
            _dotLayout = (LinearLayout) View.Inflate(context, Resource.Layout.default_indicator, null);

            return _dotLayout;
        }

        public void Initialize(int slideCount)
        {
            _dots = new List<ImageView>();
            _slideCount = slideCount;
            selectedDotColor = -1;
            unselectedDotColor = -1;

            for (int i = 0; i < slideCount; i++)
            {
                ImageView dot = new ImageView(_context);
                dot.SetImageDrawable(ContextCompat.GetDrawable(_context, Resource.Drawable.indicator_dot_grey));

                LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                _dotLayout.AddView(dot, layoutParams);
                _dots.Add(dot);
            }

            SelectPosition(FIRST_PAGE_NUM);
        }

        public void SelectPosition(int index)
        {
            _currentPosition = index;
            for (int i = 0; i < _slideCount; i++)
            {
                int drawableId;
                if (i == index)
                {
                    drawableId = (Resource.Drawable.indicator_dot_white);
                }
                else
                {
                    drawableId = (Resource.Drawable.indicator_dot_grey);
                }

                Drawable drawable = ContextCompat.GetDrawable(_context, drawableId);
                if (selectedDotColor != DEFAULT_COLOR && i == index)
                {
                    drawable.Mutate().SetColorFilter(ColorHelper.FromInt(selectedDotColor), PorterDuff.Mode.SrcIn);
                }
                if (unselectedDotColor != DEFAULT_COLOR && i != index)
                {
                    drawable.Mutate().SetColorFilter(ColorHelper.FromInt(unselectedDotColor), PorterDuff.Mode.SrcIn);
                }
                _dots[i].SetImageDrawable(drawable);
            }
        }

        public void SetSelectedIndicatorColor(int color)
        {
            selectedDotColor = color;
            SelectPosition(_currentPosition);
        }

        public void SetUnselectedIndicatorColor(int color)
        {
            unselectedDotColor = color;
            SelectPosition(_currentPosition);
        }
    }
}