using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StackOverflowNotifier
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();

			// Add icon to tab on iOS
			if (Device.OS == TargetPlatform.iOS)
			{
				Icon = "Settings.png";
			}
		}
	}
}

