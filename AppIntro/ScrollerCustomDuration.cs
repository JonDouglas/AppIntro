using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace AppIntro
{
    public class ScrollerCustomDuration : Scroller
    {
        private double _scrollFactor = 6;
        public ScrollerCustomDuration(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ScrollerCustomDuration(Context context) : base(context)
        {
        }

        public ScrollerCustomDuration(Context context, IInterpolator interpolator) : base(context, interpolator)
        {
        }

        public ScrollerCustomDuration(Context context, IInterpolator interpolator, bool flywheel) : base(context, interpolator, flywheel)
        {
        }

        public void SetScrollDurationFactor(double scrollFactor)
        {
            _scrollFactor = scrollFactor;
        }

        public override void StartScroll(int startX, int startY, int dx, int dy, int duration)
        {
            base.StartScroll(startX, startY, dx, dy, (int) (duration * _scrollFactor));
        }
    }
}