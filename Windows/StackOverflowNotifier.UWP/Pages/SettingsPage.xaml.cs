using StackOverflowNotifier.UWP.Shared.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StackOverflowNotifier.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            this.Loaded += SettingsPage_Loaded;
            App.SetColors();

            // Enable back navigation
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (sender, args) => { if (Frame.CanGoBack) Frame.GoBack(); };
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            var currentInterval =ApplicationData.Current.LocalSettings.Values["currentInterval"];
            if (currentInterval == null)
            {
                IntervalBox.SelectedItem = Interval60;
                ApplicationData.Current.LocalSettings.Values["currentInterval"] = 60;
            }
            else
            {
                switch (Convert.ToUInt32(currentInterval))
                {
                    case 0:
                        IntervalBox.SelectedItem = IntervalNever;
                        break;
                    case 30:
                        IntervalBox.SelectedItem = Interval30;
                        break;
                    default:
                    case 60:
                        IntervalBox.SelectedItem = Interval60;
                        break;
                    case 90:
                        IntervalBox.SelectedItem = Interval90;
                        break;
                    case 720:
                        IntervalBox.SelectedItem = IntervalOnce;
                        break;
                }
            }
        }

        private void IntervalBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem)e.AddedItems.First();
            uint timeInterval = 0;

            switch (selectedItem.Name)
            {
                case "IntervalNever":
                    timeInterval = 0;
                    break;
                case "Interval30":
                    timeInterval = 30;
                    break;
                case "Interval60":
                    timeInterval = 60;
                    break;
                case "Interval90":
                    timeInterval = 90;
                    break;
                case "IntervalOnce":
                    timeInterval = 720;
                    break;
            }

            BackgroundHelper.RegisterBackgroundTask(timeInterval);
            ApplicationData.Current.LocalSettings.Values["currentInterval"] = timeInterval;            
        }
    }
}
