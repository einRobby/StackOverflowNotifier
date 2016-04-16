using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;

namespace StackOverflowNotifier.Droid
{
    [Activity(Label = "StackOverflowNotifier", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
			// Because we are using AppCompat and Material here, we have to tell Xamarin.Forms where to find the toolbar for more customization
			FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
			FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

			base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

