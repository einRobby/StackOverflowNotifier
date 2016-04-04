using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace StackOverflowNotifier
{
    public class App : Application
    {
		public App()
		{
			// The root page of your application
			MainPage = new NavigationPage(new MainPage()) 
			{
				BarBackgroundColor = Color.FromHex("f37e22"),
				BarTextColor = Color.White
			};

		}

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
