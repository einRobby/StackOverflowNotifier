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
		public MainViewModel MainViewModel;

		public MainPage()
		{
			InitializeComponent();

			MainViewModel = new MainViewModel(new UrlService(), new StackOverflowService(new HttpService()));
			BindingContext = MainViewModel;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await MainViewModel.RefreshAsync();
		}

		// TODO: Can't we do this via Command Bindings?
		public void Question_Selected(object sender, SelectedItemChangedEventArgs args)
		{
			var selectedQuestion = args.SelectedItem as Question;
			if (selectedQuestion != null)
			{				
				MainViewModel.OpenQuestionCommand.Execute(selectedQuestion);
			}
		}
	}
}