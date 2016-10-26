using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Java.Lang.Reflect;

namespace AppIntro
{
    [Register("com.jondouglas.appintro.AppIntroViewPager")]
    public class AppIntroViewPager : ViewPager
    {
        private static int ON_ILLEGALLY_REQUESTED_NEXT_PAGE_MAX_INTERVAL = 1000;
        private bool pagingEnabled;
        private bool nextPagingEnabled;
        private float currentTouchDownX;
        private long illegallyRequestedNextPageLastCalled;
        private int lockPage;
        private ScrollerCustomDuration mScroller = null;
        private IOnNextPageRequestedListener nextPageRequestedListener;
        private AppIntroBase.PagerOnPageChangeListener pageChangeListener;

        public AppIntroViewPager(Context context, IAttributeSet attributeSet) : base(context, attributeSet)
        {
            pagingEnabled = true;
            nextPagingEnabled = true;
            lockPage = 0;

            InitViewPagerScroller();
        }

        public void AddOnPageChangeListener(AppIntroBase.PagerOnPageChangeListener listener)
        {
            base.AddOnPageChangeListener(listener);

            this.pageChangeListener = listener;
        }

        public override void SetCurrentItem(int item, bool smoothScroll)
        {
            bool invokeMeLater = base.CurrentItem == 0 && item == 0;

            base.SetCurrentItem(item, smoothScroll);

            if (invokeMeLater && pageChangeListener != null)
            {
                pageChangeListener.OnPageSelected(0);
            }
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            if (ev.Action == MotionEventActions.Down)
            {
                currentTouchDownX = ev.GetX();
                return base.OnInterceptTouchEvent(ev);
            }
            else if (CheckPagingState(ev) || CheckCanRequestNextPage(ev))
            {
                CheckIllegallyRequestedNextPage(ev);
                return false;
            }

            return base.OnInterceptTouchEvent(ev);
        }

        public override bool OnTouchEvent(MotionEvent ev)
        {
            if (ev.Action == MotionEventActions.Down)
            {
                currentTouchDownX = ev.GetX();
                return base.OnTouchEvent(ev);
            }
            else if (CheckPagingState(ev) || CheckCanRequestNextPage(ev))
            {
                CheckIllegallyRequestedNextPage(ev);
                return false;
            }

            return base.OnTouchEvent(ev);
        }

        private bool CheckPagingState(MotionEvent ev)
        {
            if (!pagingEnabled)
            {
                return true;
            }

            if (!nextPagingEnabled)
            {
                if (ev.Action == MotionEventActions.Down)
                {
                    currentTouchDownX = ev.GetX();
                }
                if (ev.Action == MotionEventActions.Move)
                {
                    if (DetectSwipeToRight(ev))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckCanRequestNextPage(MotionEvent ev)
        {
            return nextPageRequestedListener != null && !nextPageRequestedListener.OnCanRequestNextPage();
        }

        private void CheckIllegallyRequestedNextPage(MotionEvent ev)
        {
            int swipeThreshold = 25;

            if (ev.Action == MotionEventActions.Move && Math.Abs(ev.GetX() - currentTouchDownX) >= swipeThreshold)
            {
                if (Java.Lang.JavaSystem.CurrentTimeMillis() - illegallyRequestedNextPageLastCalled >=
                    ON_ILLEGALLY_REQUESTED_NEXT_PAGE_MAX_INTERVAL)
                {
                    illegallyRequestedNextPageLastCalled = Java.Lang.JavaSystem.CurrentTimeMillis();

                    if (nextPageRequestedListener != null)
                    {
                        nextPageRequestedListener.OnIllegallyRequestedNextPage();
                    }
                }
            }
        }

        private void InitViewPagerScroller()
        {
            //CONVERT VIA REFLECTION
//            try
//            {
//                Field scroller = ViewPager.class.getDeclaredField("mScroller");
//        scroller.setAccessible(true);
//            Field interpolator = ViewPager.class.getDeclaredField("sInterpolator");
//        interpolator.setAccessible(true);
//
//            mScroller = new ScrollerCustomDuration(getContext(),
//                    (Interpolator) interpolator.get(null));
//            scroller.set(this, mScroller);
//        } catch (Exception e) {
//            e.printStackTrace();
//        }
        }

        private bool DetectSwipeToRight(MotionEvent ev)
        {
            int SWIPE_THRESHOLD = 0;
            bool result = false;

            try
            {
                float diffx = ev.GetX() - currentTouchDownX;
                if (Math.Abs(diffx) > SWIPE_THRESHOLD)
                {
                    if (diffx > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return result;
        }

        public void SetOnNextPageRequestedListener(IOnNextPageRequestedListener nextPageRequestedListener)
        {
            this.nextPageRequestedListener = nextPageRequestedListener;
        }

        public void SetScrollDurationFactor(double scrollFactor)
        {
            mScroller.SetScrollDurationFactor(scrollFactor);
        }

        public bool IsNextPagingEnabled()
        {
            return nextPagingEnabled;
        }

        public void SetNextPagingEnabled(bool nextPagingEnabled)
        {
            this.nextPagingEnabled = nextPagingEnabled;
            if (!nextPagingEnabled)
            {
                lockPage = CurrentItem;
            }
        }

        public bool IsPagingEnabled()
        {
            return pagingEnabled;
        }

        public void SetPagingEnabled(bool pagingEnabled)
        {
            this.pagingEnabled = pagingEnabled;
        }

        public int GetLockPage()
        {
            return lockPage;
        }

        public void SetLockPage(int lockPage)
        {
            this.lockPage = lockPage;
        }
    }

        public interface IOnNextPageRequestedListener
    {
        bool OnCanRequestNextPage();

        void OnIllegallyRequestedNextPage();
    }
}