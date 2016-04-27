using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StackOverflowNotifier
{
	public partial class TagPage : ContentPage
	{
		public TagPage()
		{
			InitializeComponent();

			// On Android, add the settings button to the toolbar. On iOS it has been added to the tab bar
			if (Device.OS == TargetPlatform.Android)
			{
				ToolbarItems.Add(new ToolbarItem("Settings", "Settings.png", new Action(() => { App.Bootstrapper.MainViewModel.NavigateToSettingsCommand.Execute(null); }), ToolbarItemOrder.Primary, 0));
			}

			// Add icon to tab on iOS
			if (Device.OS == TargetPlatform.iOS)
			{
				Icon = "Tag.png";
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = App.Bootstrapper.MainViewModel;
		}

		public void TagEntry_Completed(object sender, EventArgs e)
		{
			App.Bootstrapper.MainViewModel.AddTagCommand.Execute(TagEntry.Text);
			TagEntry.Text = string.Empty;
		}


		public void RemoveEntry_Clicked(object sender, EventArgs e)
		{
			App.Bootstrapper.MainViewModel.RemoveTagCommand.Execute((sender as Button).BindingContext);
		}
	}
}

