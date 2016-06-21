using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using StackOverflowNotifier.Shared;
using StackOverflowNotifier.Shared.Models;
using Xamarin.Forms;

namespace StackOverflowNotifier
{
	public partial class MainPage : ContentPage
	{

		public MainPage()
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
				Icon = "Question.png";
			}
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = App.Bootstrapper.MainViewModel;
			lblNewQuestionCount.BindingContext = App.Bootstrapper.MainViewModel;

			await App.Bootstrapper.MainViewModel.RefreshAsync();
		}

		// TODO: Can't we do this via Command Bindings?
		public void Question_Selected(object sender, SelectedItemChangedEventArgs args)
		{
			var selectedQuestion = args.SelectedItem as Question;
			if (selectedQuestion != null)
			{				
				App.Bootstrapper.MainViewModel.OpenQuestionCommand.Execute(selectedQuestion);
				(sender as ListView).SelectedItem = null;
			}
		}
	}
}