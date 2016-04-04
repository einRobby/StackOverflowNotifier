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
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = App.Bootstrapper.MainViewModel;
		}
	}
}

