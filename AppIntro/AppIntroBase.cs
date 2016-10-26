using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using AppIntro.Utils;
using Java.Lang;
using Java.Util;
using Fragment = Android.Support.V4.App.Fragment;

namespace AppIntro
{
    public abstract class AppIntroBase : AppCompatActivity, IOnNextPageRequestedListener
    {
        public static int DEFAULT_COLOR = 1;

        //private static string TAG = LogHelper.MakeLogTag(AppIntroBase.class);

        private static int DEFAULT_SCROLL_DURATION_FACTOR = 1;

        private const int PERMISSIONS_REQUEST_ALL_PERMISSIONS = 1;

        private static string INSTANCE_DATA_IMMERSIVE_MODE_ENABLED =
            "com.github.paolorotolo.appintro_immersive_mode_enabled";

        private static string INSTANCE_DATA_IMMERSIVE_MODE_STICKY =
            "com.github.paolorotolo.appintro_immersive_mode_sticky";

        private static string INSTANCE_DATA_COLOR_TRANSITIONS_ENABLED =
            "com.github.paolorotolo.appintro_color_transitions_enabled";

        protected List<Fragment> fragments = new List<Fragment>();

        private ArgbEvaluator argbEvaluator = new ArgbEvaluator();

        protected PagerAdapter _pagerAdapter;

        protected AppIntroViewPager _pager;

        protected Vibrator _vibrator;

        protected IndicatorController _controller;

        protected int slidesNumber;

        protected int vibrateIntensity = 20;

        protected int selectedIndicatorColor = DEFAULT_COLOR;

        protected int unselectedIndicatorColor = DEFAULT_COLOR;

        protected View nextButton;

        protected View doneButton;

        protected View skipButton;

        protected View backButton;

        protected int savedCurrentItem;

        protected List<PermissionObject> permissionsArray = new List<PermissionObject>();

        protected bool isVibrateOn = false;

        protected bool baseProgressButtonEnabled = true;

        protected bool progressButtonEnabled = true;

        protected bool skipButtonEnabled = true;

        protected bool isWizardMode = false;

        protected bool showBackButtonWithDone = false;

        private GestureDetectorCompat gestureDetector;
        private bool isGoBackLockEnabled = false;
        private bool isImmersiveModeEnabled = false;
        private bool isImmersiveModeSticky = false;
        private bool areColorTransitionsEnabled = false;
        private int currentlySelectedItem = -1;

        protected abstract int LayoutId { get; }

    protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(LayoutId);

            gestureDetector = new GestureDetectorCompat(this, new WindowGestureListener(this));

            nextButton = FindViewById(Resource.Id.next);

            doneButton = FindViewById(Resource.Id.done);

            skipButton = FindViewById(Resource.Id.skip);

            backButton = FindViewById(Resource.Id.back);

            _vibrator = (Vibrator) this.GetSystemService(VibratorService);

            _pagerAdapter = new PagerAdapter(SupportFragmentManager, fragments);

            _pager = (AppIntroViewPager) FindViewById(Resource.Id.view_pager);

            doneButton.Click += DoneButtonOnClick;

            skipButton.Click += SkipButtonOnClick;

            nextButton.Click += NextButtonOnClick;

            backButton.Click += BackButtonOnClick;

            _pager.Adapter = _pagerAdapter;
            //Port over https://github.com/PaoloRotolo/AppIntro/blob/master/library/src/main/java/com/github/paolorotolo/appintro/AppIntroBase.java#L870-L938
            // To the respective events already exposed
            //            _pager.addOnPageChangeListener(new PagerOnPageChangeListener());
            //            _pager.setOnNextPageRequestedListener(this);
            //
            //            setScrollDurationFactor(DEFAULT_SCROLL_DURATION_FACTOR);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            if (fragments.Count == 0)
            {
                Init(null);
            }

            _pager.CurrentItem = savedCurrentItem;
            _pager.Post(() => new Runnable(() => HandleSlideChanged(null, _pagerAdapter.GetItem(_pager.CurrentItem))));

            slidesNumber = fragments.Count;
            SetProgressButtonEnabled(progressButtonEnabled);
            InitController();
        }

        public override void OnBackPressed()
        {
            if(!isGoBackLockEnabled)
                base.OnBackPressed();
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (hasFocus && isImmersiveModeEnabled)
            {
                SetImmersiveMode(true, isImmersiveModeSticky);
            }
        }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            if (isImmersiveModeEnabled)
            {
                gestureDetector.OnTouchEvent(ev);
            }

            return base.DispatchTouchEvent(ev);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutBoolean("baseProgressButtonEnabled", baseProgressButtonEnabled);
            outState.PutBoolean("progressButtonEnabled", progressButtonEnabled);
            outState.PutBoolean("nextEnabled", _pager.IsPagingEnabled());
            outState.PutBoolean("nextPagingEnabled", _pager.IsNextPagingEnabled());
            outState.PutBoolean("skipButtonEnabled", skipButtonEnabled);
            outState.PutInt("lockPage", _pager.GetLockPage());
            outState.PutInt("currentItem", _pager.CurrentItem);

            outState.PutBoolean(INSTANCE_DATA_IMMERSIVE_MODE_ENABLED, isImmersiveModeEnabled);
            outState.PutBoolean(INSTANCE_DATA_IMMERSIVE_MODE_STICKY, isImmersiveModeSticky);
            outState.PutBoolean(INSTANCE_DATA_COLOR_TRANSITIONS_ENABLED, areColorTransitionsEnabled);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);

            this.baseProgressButtonEnabled = savedInstanceState.GetBoolean("baseProgressButtonEnabled");
            this.progressButtonEnabled = savedInstanceState.GetBoolean("progressButtonEnabled");
            this.skipButtonEnabled = savedInstanceState.GetBoolean("skipButtonEnabled");
            this.savedCurrentItem = savedInstanceState.GetInt("currentItem");
            _pager.SetPagingEnabled(savedInstanceState.GetBoolean("nextEnabled"));
            _pager.SetNextPagingEnabled(savedInstanceState.GetBoolean("nextPagingEnabled"));
            _pager.SetLockPage(savedInstanceState.GetInt("lockPage"));

            isImmersiveModeEnabled = savedInstanceState.GetBoolean(INSTANCE_DATA_IMMERSIVE_MODE_ENABLED);
            isImmersiveModeSticky = savedInstanceState.GetBoolean(INSTANCE_DATA_IMMERSIVE_MODE_STICKY);
            areColorTransitionsEnabled = savedInstanceState.GetBoolean(
                    INSTANCE_DATA_COLOR_TRANSITIONS_ENABLED);
        }

        public bool OnCanRequestNextPage()
        {
            return HandleBeforeSlideChanged();
        }

        public void OnIllegallyRequestedNextPage()
        {
            HandleIllegalSlideChangeAttempt();
        }

        private void InitController()
        {
            if (_controller == null)
            {
                _controller = new DefaultIndicatorController();
            }

            FrameLayout indicatorContainer = (FrameLayout) FindViewById(Resource.Id.indicator_container);
            indicatorContainer.AddView(_controller.NewInstance(this));

            _controller.Initialize(slidesNumber);
            if (selectedIndicatorColor != DEFAULT_COLOR)
            {
                _controller.SetSelectedIndicatorColor(selectedIndicatorColor);
            }
            if (unselectedIndicatorColor != DEFAULT_COLOR)
            {
                _controller.SetUnselectedIndicatorColor(unselectedIndicatorColor);
            }

            _controller.SelectPosition(currentlySelectedItem);
        }

        private void HandleIllegalSlideChangeAttempt()
        {
            Fragment currentFragment = _pagerAdapter.GetItem(_pager.CurrentItem);

            if (currentFragment != null && currentFragment is ISlidePolicy)
            {
                ISlidePolicy slide = (ISlidePolicy) currentFragment;

                if (!slide.IsPolicyRespected())
                {
                    slide.OnUserIllegallyRequestedNextPage();
                }
            }
        }

        private bool HandleBeforeSlideChanged()
        {
            Fragment currentFragment = _pagerAdapter.GetItem(_pager.CurrentItem);

//            LogHelper.d(TAG, String.format(
//                "User wants to move away from slide: %s. Checking if this should be allowed...",
//                currentFragment));

            if (currentFragment is ISlidePolicy)
            {
                ISlidePolicy slide = (ISlidePolicy) currentFragment;

//                LogHelper.d(TAG, "Current fragment implements ISlidePolicy.");

                if (!slide.IsPolicyRespected())
                {
//                    LogHelper.d(TAG, "Slide policy not respected, denying change request.");
                    return false;
                }
            }

            //LogHelper.d(TAG, "Change request will be allowed.");
            return true;
        }

        private void HandleSlideChanged(Fragment oldFragment, Fragment newFragment)
        {
            if (oldFragment != null && oldFragment is ISlideSelectionListener)
            {
                ((ISlideSelectionListener) oldFragment).OnSlideDeselected();
            }

            if (newFragment != null && newFragment is ISlideSelectionListener)
            {
                ((ISlideSelectionListener)newFragment).OnSlideSelected();
            }

            OnSlideChanged(oldFragment, newFragment);
        }

        protected void OnPageSelected(int position)
        {
            
        }

        public void ShowSkipButton(bool showButton)
        {
            this.skipButtonEnabled = showButton;
            SetButtonState(skipButton, showButton);
        }

        public bool IsSkipButtonEnabled()
        {
            return skipButtonEnabled;
        }

        public virtual void OnSkipPressed(Fragment currentFragment)
        {
            OnSkipPressed();
        }

        protected void SetScrollDurationFactor(int factor)
        {
            _pager.SetScrollDurationFactor(factor);
        }

        protected void SetButtonState(View button, bool show)
        {
            if (show)
            {
                button.Visibility = ViewStates.Visible;
            }
            else
            {
                button.Visibility = ViewStates.Invisible;
            }
        }

        public AppIntroViewPager GetPager()
        {
            return _pager;
        }

        public List<Fragment> GetSlides()
        {
            return _pagerAdapter.GetFragments();
        }

        public void AddSlide(Fragment fragment)
        {
            fragments.Add(fragment);
            if (isWizardMode)
            {
                SetOffScreenPageLimit(fragments.Count);
            }
            _pagerAdapter.NotifyDataSetChanged();
        }

        public bool IsProgressButtonEnabled()
        {
            return progressButtonEnabled;
        }

        public void SetProgressButtonEnabled(bool progressButtonEnabled)
        {
            this.progressButtonEnabled = progressButtonEnabled;
            if (progressButtonEnabled)
            {
                if (_pager.CurrentItem == slidesNumber - 1)
                {
                    SetButtonState(nextButton, false);
                    SetButtonState(doneButton, true);
                    if (isWizardMode)
                    {
                        SetButtonState(backButton, showBackButtonWithDone);
                    }
                    else
                    {
                        SetButtonState(skipButton, false);
                    }
                }
                else
                {
                    SetButtonState(nextButton, true);
                    SetButtonState(doneButton, false);
                    if (isWizardMode)
                    {
                        if (_pager.CurrentItem == 0)
                        {
                            SetButtonState(backButton, false);
                        }
                        else
                        {
                            SetButtonState(backButton, isWizardMode);
                        }
                    }
                    else
                    {
                        SetButtonState(skipButton, skipButtonEnabled);
                    }
                }
            }
            else
            {
                SetButtonState(nextButton, false);
                SetButtonState(doneButton, false);
                SetButtonState(backButton, false);
                SetButtonState(skipButton, false);
            }
        }

        public void SetOffScreenPageLimit(int limit)
        {
            _pager.OffscreenPageLimit = limit;
        }

        public virtual void Init()
        {

        }

        public virtual void Init(Bundle savedInstanceState)
        {
            
        }

        public virtual void OnNextPressed()
        {
            
        }

        public virtual void OnDonePressed()
        {
            
        }

        public virtual void OnSkipPressed()
        {
            
        }

        public virtual void OnSlideChanged()
        {
            
        }

        public void OnDonePressed(Fragment currentFragment)
        {
            OnDonePressed();
        }

        public void OnSlideChanged(Fragment oldFragment, Fragment newFragment)
        {
            OnSlideChanged();
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Enter || keyCode == Keycode.A || keyCode == Keycode.DpadCenter)
            {
                ViewPager vp = (ViewPager) this.FindViewById(Resource.Id.view_pager);
                if (vp.CurrentItem == vp.Adapter.Count - 1)
                {
                    OnDonePressed(fragments[vp.CurrentItem]);
                }
                else
                {
                    vp.CurrentItem = vp.CurrentItem + 1;
                }

                return false;
            }

            return base.OnKeyDown(keyCode, e);
        }

        public void SetNavBarColor(string color)
        {
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                Window.SetNavigationBarColor(Color.ParseColor(color));
            }
        }

        public void SetNavBarColor(int color)
        {
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                Window.SetNavigationBarColor(ColorHelper.FromInt(color));
            }
        }

        public void ShowStatusBar(bool isVisible)
        {
            if (!isVisible)
            {
                Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            }
            else
            {
                Window.ClearFlags(WindowManagerFlags.Fullscreen);
            }
        }

        public void SetVibrate(bool vibrationEnabled)
        {
            this.isVibrateOn = vibrationEnabled;
        }

        public bool GetWizardMode()
        {
            return isWizardMode;
        }

        public void SetWizardMode(bool wizardMode)
        {
            this.isWizardMode = wizardMode;
            this.skipButtonEnabled = false;
            SetButtonState(skipButton, !wizardMode);
        }

        public bool GetBackButtonVisibilityWithDone()
        {
            return isWizardMode;
        }

        public void SetBackButtonVisibilityWithDone(bool show)
        {
            this.showBackButtonWithDone = show;
        }

        public void SetVibrateIntensity(int intensity)
        {
            this.vibrateIntensity = intensity;
        }

        public void SetProgressIndicator()
        {
            _controller = new ProgressIndicatorController();
        }

        public void SetCustomIndicator(IndicatorController controller)
        {
            _controller = controller;
        }

        public void SetColorTransitionsEnabled(bool colorTransitionsEnabled)
        {
            areColorTransitionsEnabled = colorTransitionsEnabled;
        }

        public void SetFadeAnimation()
        {
            _pager.SetPageTransformer(true, new ViewPageTransformer(TransformType.FADE));
        }

        public void SetZoomAnimation()
        {
            _pager.SetPageTransformer(true, new ViewPageTransformer(TransformType.ZOOM));
        }

        public void SetFlowAnimation()
        {
            _pager.SetPageTransformer(true, new ViewPageTransformer(TransformType.FLOW));
        }

        public void SetSlideOverAnimation()
        {
            _pager.SetPageTransformer(true, new ViewPageTransformer(TransformType.SLIDE_OVER));
        }

        public void SetDepthAnimation()
        {
            _pager.SetPageTransformer(true, new ViewPageTransformer(TransformType.DEPTH));
        }

        public void SetCustomTransformer(ViewPager.IPageTransformer transformer)
        {
            _pager.SetPageTransformer(true, transformer);
        }

        public void SetIndicatorColor(int selectedIndicatorColor, int unselectedIndicatorColor)
        {
            this.selectedIndicatorColor = selectedIndicatorColor;
            this.unselectedIndicatorColor = unselectedIndicatorColor;

            if (_controller != null)
            {
                if (selectedIndicatorColor != DEFAULT_COLOR)
                {
                    _controller.SetSelectedIndicatorColor(selectedIndicatorColor);
                }

                if (unselectedIndicatorColor != DEFAULT_COLOR)
                {
                    _controller.SetUnselectedIndicatorColor(unselectedIndicatorColor);
                }
            }
        }

        public void SetNextPageSwipeLock(bool lockEnable)
        {
            if (lockEnable)
            {
                baseProgressButtonEnabled = progressButtonEnabled;
                SetProgressButtonEnabled(false);
            }
            else
            {
                SetProgressButtonEnabled(baseProgressButtonEnabled);
            }

            _pager.SetNextPagingEnabled(!lockEnable);
        }

        public void SetSwipeLock(bool lockEnable)
        {
            if (lockEnable)
            {
                baseProgressButtonEnabled = progressButtonEnabled;
            }
            else
            {
                SetProgressButtonEnabled(baseProgressButtonEnabled);
            }
            _pager.SetPagingEnabled(!lockEnable);
        }

        public void SetGoBackLock(bool lockEnabled)
        {
            isGoBackLockEnabled = lockEnabled;
        }

        public void SetImmersiveMode(bool isEnabled)
        {
            SetImmersiveMode(isEnabled, false);
        }

        public void SetImmersiveMode(bool isEnabled, bool isSticky)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                if (!isEnabled && isImmersiveModeEnabled)
                {
                    Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.LayoutStable |
                                                          SystemUiFlags.LayoutHideNavigation |
                                                          SystemUiFlags.LayoutFullscreen);

                    isImmersiveModeEnabled = false;
                }
                else if (isEnabled)
                {
                    SystemUiFlags flags = SystemUiFlags.LayoutStable |
                                SystemUiFlags.LayoutHideNavigation |
                                SystemUiFlags.LayoutFullscreen |
                                SystemUiFlags.HideNavigation |
                                SystemUiFlags.Fullscreen;

                    if (isSticky)
                    {
                        flags |= SystemUiFlags.ImmersiveSticky;
                        isImmersiveModeSticky = true;
                    }
                    else
                    {
                        flags |= SystemUiFlags.Immersive;
                        isImmersiveModeSticky = false;
                    }

                    Window.DecorView.SystemUiVisibility = (StatusBarVisibility)flags;

                    isImmersiveModeEnabled = true;
                }
            }
        }

        public void AskForPermissions(string[] permissions, int slidesNumber)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (slidesNumber == 0)
                {
                    Toast.MakeText(BaseContext, "Invalid Slide Number", ToastLength.Short).Show();
                }
                else
                {
                    PermissionObject permission = new PermissionObject(permissions, slidesNumber);
                    permissionsArray.Add(permission);
                    SetSwipeLock(true);
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            switch (requestCode)
            {
                case PERMISSIONS_REQUEST_ALL_PERMISSIONS:
                    _pager.CurrentItem = _pager.CurrentItem + 1;
                    break;
                default:
                    //LogHelper.e(TAG, "Unexpected request code");
                    break;
            }
        }



        private void DoneButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (isVibrateOn)
            {
                _vibrator.Vibrate(vibrateIntensity);
            }

            Fragment currentFragment = _pagerAdapter.GetItem(_pager.CurrentItem);
            bool isSlideChangingAllowed = HandleBeforeSlideChanged();

            if (isSlideChangingAllowed)
            {
                HandleSlideChanged(currentFragment, null);
                OnDonePressed(currentFragment);
            }
            else
            {
                HandleIllegalSlideChangeAttempt();
            }
        }

        private void SkipButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (isVibrateOn)
            {
                _vibrator.Vibrate(vibrateIntensity);
            }

            OnSkipPressed(_pagerAdapter.GetItem(_pager.CurrentItem));
        }

        private void NextButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (isVibrateOn)
            {
                _vibrator.Vibrate(vibrateIntensity);
            }

            bool isSlideChangingAllowed = HandleBeforeSlideChanged();

            if (isSlideChangingAllowed)
            {
                bool requestPermission = false;
                int position = 0;

                for (int i = 0; i < permissionsArray.Count; i++)
                {
                    requestPermission = _pager.CurrentItem + 1 == permissionsArray[i].GetPosition();
                    position = i;
                    break;
                }

                if (requestPermission)
                {
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                    {
                        RequestPermissions(permissionsArray[position].GetPermission(),
                            PERMISSIONS_REQUEST_ALL_PERMISSIONS);

                        permissionsArray.RemoveAt(position);
                    }
                    else
                    {
                        _pager.CurrentItem = _pager.CurrentItem + 1;
                        OnNextPressed();
                    }
                }
                else
                {
                    _pager.CurrentItem = _pager.CurrentItem + 1;
                    OnNextPressed();
                }
            }
            else
            {
                HandleIllegalSlideChangeAttempt();
            }
        }

        private void BackButtonOnClick(object sender, EventArgs eventArgs)
        {
            if (_pager.CurrentItem > 0)
            {
                _pager.CurrentItem = _pager.CurrentItem - 1;
            }
        }

        public class PagerOnPageChangeListener : Java.Lang.Object, ViewPager.IOnPageChangeListener
        {
            private AppIntroBase appIntroBase;

            public void OnPageScrollStateChanged(int state)
            {
            }

            public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
            {
                throw new NotImplementedException();
            }

            public void OnPageSelected(int position)
            {
                if (appIntroBase.slidesNumber > 1)
                {
                    appIntroBase._controller.SelectPosition(position);
                }

                if (appIntroBase._pager.IsNextPagingEnabled())
                {
                    if (appIntroBase._pager.CurrentItem != appIntroBase._pager.GetLockPage())
                    {
                        appIntroBase.SetProgressButtonEnabled(appIntroBase.baseProgressButtonEnabled);
                        appIntroBase._pager.SetNextPagingEnabled(true);
                    }
                    else
                    {
                        appIntroBase.SetProgressButtonEnabled(appIntroBase.progressButtonEnabled);
                    }
                }
                else
                {
                    appIntroBase.SetProgressButtonEnabled(appIntroBase.progressButtonEnabled);
                }

                appIntroBase.OnPageSelected(position);

                if (appIntroBase.slidesNumber > 0)
                {
                    if (appIntroBase.currentlySelectedItem == -1)
                    {
                        appIntroBase.HandleSlideChanged(null, appIntroBase._pagerAdapter.GetItem(position));
                    }
                    else
                    {
                        appIntroBase.HandleSlideChanged(appIntroBase._pagerAdapter.GetItem(appIntroBase.currentlySelectedItem), appIntroBase._pagerAdapter.GetItem(appIntroBase._pager.CurrentItem));
                    }
                }

                appIntroBase.currentlySelectedItem = position;
            }
        }

        private class WindowGestureListener : GestureDetector.SimpleOnGestureListener
        {
            private AppIntroBase appIntroBase;
            public WindowGestureListener(AppIntroBase appIntroBase)
            {
                this.appIntroBase = appIntroBase;
            }
            public override bool OnSingleTapUp(MotionEvent e)
            {
                if (appIntroBase.isImmersiveModeEnabled && !appIntroBase.isImmersiveModeSticky)
                {
                    appIntroBase.SetImmersiveMode(true, false);
                }

                return false;
            }
        }

    }





}
