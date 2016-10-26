using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Java.Interop;

namespace Sample
{
    [Activity(Label = "Sample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_main);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }

        [Export("startDefaultIntro")]
        public void StartDefaultIntro(View v)
        {
            Intent intent = new Intent(this, typeof(DefaultIntro));
            StartActivity(intent);
        }
    }
}

