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

			// Toolbar
			ToolbarItems.Add(new ToolbarItem("Tags", "", new Action(() => { App.Bootstrapper.MainViewModel.NavigateToTagsCommand.Execute(null); }), ToolbarItemOrder.Primary, 0));
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = App.Bootstrapper.MainViewModel;

			await App.Bootstrapper.MainViewModel.RefreshAsync();
		}

		// TODO: Can't we do this via Command Bindings?
		public void Question_Selected(object sender, SelectedItemChangedEventArgs args)
		{
			var selectedQuestion = args.SelectedItem as Question;
			if (selectedQuestion != null)
			{				
				App.Bootstrapper.MainViewModel.OpenQuestionCommand.Execute(selectedQuestion);
			}
		}
	}
}