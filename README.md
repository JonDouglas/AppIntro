<p>Sample App:</p>
<a href="https://play.google.com/store/apps/details?id=paolorotolo.github.com.appintroexample&utm_source=global_co&utm_medium=prtnr&utm_content=Mar2515&utm_campaign=PartBadge&pcampaignid=MKT-AC-global-none-all-co-pr-py-PartBadges-Oct1515-1"><img alt="Get it on Google Play" src="https://play.google.com/intl/en_us/badges/images/apps/en-play-badge-border.png" width="300" /></a>

# AppIntro
AppIntro is an Android Library that helps you make a **cool intro** for your app, like the ones in Google apps.

*Watch YouTube video [here](https://www.youtube.com/watch?v=OlAugnH3jFY&feature=youtu.be).*

<img src="https://github.com/PaoloRotolo/AppIntro/blob/master/art/intro.png" width="300">
<img src="https://github.com/PaoloRotolo/AppIntro/blob/master/art/layout2.png" width="300">

## Usage

### Basic usage

Add this project to your Xamarin.Android solution!

Create a new `Activity` that extends `AppIntro`:

```c#
 [Activity(Theme = "@style/FullscreenTheme")]
    public class DefaultIntro : BaseIntro
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AddSlide(SampleSlide.NewInstance(Resource.Layout.intro));
            AddSlide(SampleSlide.NewInstance(Resource.Layout.intro2));
            AddSlide(SampleSlide.NewInstance(Resource.Layout.intro3));
            AddSlide(SampleSlide.NewInstance(Resource.Layout.intro4));
        }

        public void GetStarted(View v)
        {
            LoadMainActivity();
        }

        public override void OnDonePressed()
        {
            base.OnDonePressed();

            LoadMainActivity();
        }

        public override void OnSkipPressed(Fragment currentFragment)
        {
            base.OnSkipPressed();
            LoadMainActivity();
            Toast.MakeText(ApplicationContext, Resource.String.skip, ToastLength.Short).Show();
        }
    }
```

_Note above that we DID NOT use `SetContentView();`_

Finally, declare the activity in your Manifest like so:

```c#
 [Activity(Theme = "@style/FullscreenTheme")]
```

Do not declare the intro as your main app launcher unless you want the intro to launch every time your app starts.
Refer to the [wiki](https://github.com/PaoloRotolo/AppIntro/wiki/How-to-Use#show-the-intro-once) for an example of how to launch the intro once from your main activity.

#### Alternative layout
If you want to try an alternative layout (as seen in Google's Photo app), just extend `AppIntro2` in your Activity. That's all :)

```c#
public class IntroActivity : AppIntro2 
{
    // ...
}
```

<img src="https://github.com/PaoloRotolo/AppIntro/blob/master/art/layout2.png" width="300">
<img src="https://github.com/PaoloRotolo/AppIntro/blob/master/art/layout2_2.png" width="300">
<br>

#### Slides

##### Basic slides

AppIntro provides two simple classes, `AppIntroFragment` and `AppIntro2Fragment` which one can use to build simple slides.

```c#
protected override void OnCreate(Bundle savedInstanceState)
{
    // ...

    AddSlide(AppIntroFragment.NewInstance(title, description, image, backgroundColor));
}
```

##### Custom slides example

One may also define custom slides as seen in the example project:
 * Copy the class **SampleSlide** from my [example project](https://github.com/PaoloRotolo/AppIntro/blob/master/example/src/main/java/com/github/paolorotolo/appintroexample/SampleSlide.java).
 * Add a new slide with `addSlide(SampleSlide.newInstance(R.layout.your_slide_here));`

There's no need to create one class for fragment anymore. :)

### Extended usage

See the readme on the original project for more information:

https://github.com/PaoloRotolo/AppIntro#extended-usage

