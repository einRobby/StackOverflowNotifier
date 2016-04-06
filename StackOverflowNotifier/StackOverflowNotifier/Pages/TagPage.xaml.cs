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

			// Toolbar
			ToolbarItems.Add(new ToolbarItem("Settings", "Settings.png", new Action(() => { App.Bootstrapper.MainViewModel.NavigateToSettingsCommand.Execute(null); }), ToolbarItemOrder.Primary, 0));
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = App.Bootstrapper.MainViewModel;
		}
	}
}

