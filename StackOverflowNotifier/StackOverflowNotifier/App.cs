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
		public static Bootstrapper Bootstrapper;

		public App()
		{
			Bootstrapper = new Bootstrapper();

			// The root page of your application
			var tabbedPage = new TabbedPage();
			tabbedPage.Title = "Stack Overflow Notifier";
			tabbedPage.Children.Add(new MainPage() { Icon = "Question.png" });
			tabbedPage.Children.Add(new TagPage() { Icon = "Tag.png" });

			MainPage = new NavigationPage(tabbedPage)
			{
				BarBackgroundColor = Color.FromHex("f37e22"),
				BarTextColor = Color.White
			};

			Bootstrapper.InitalizeNavigationService(MainPage);
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
