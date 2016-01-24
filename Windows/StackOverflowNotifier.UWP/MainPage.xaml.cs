using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI;
using StackOverflowNotifier.UWP.Shared.ViewModels;
using StackOverflowNotifier.Shared.Models;
using Windows.ApplicationModel.Background;
using StackOverflowNotifier.UWP.Shared.Tools;
using StackOverflowNotifier.UWP.Pages;
using Windows.Storage;
using System.Net.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StackOverflowNotifier.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            // Initialize page
            this.InitializeComponent();
            App.SetColors();

            // Register background task
            var currentInterval = ApplicationData.Current.LocalSettings.Values["currentInterval"];
            if (currentInterval != null)
                BackgroundHelper.RegisterBackgroundTask(Convert.ToUInt32(currentInterval));
            else
                BackgroundHelper.RegisterBackgroundTask(60);


            // Clear notifications
            NotificationHelper.DeleteAllNotifications();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await MainViewModel.Current.LoadTagsAsync();
            await ReloadQuestions();
        }

        private async Task ReloadQuestions()
        {
            ProgressIndicator.Visibility = Visibility.Visible;
            ConnectionFailedMessage.Visibility = Visibility.Collapsed;

            try
            {
                await MainViewModel.Current.LoadQuestionsAsync();
            }
            catch (HttpRequestException httpRequestException)
            {
                ConnectionFailedMessage.Visibility = Visibility.Visible;
            }

            ProgressIndicator.Visibility = Visibility.Collapsed;
        }

        private async void QuestionList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Question;
            if (item != null)
            {
                item.IsNew = false;
                await Launcher.LaunchUriAsync(new Uri(item.Link));
            }
        }

        private async void TagsButton_Click(object sender, RoutedEventArgs e)
        {
            await TagsDialog.ShowAsync();
            NotificationHelper.ShowSimpleToastNotification("ads", "asd");
        }

        private void DeleteTagButton_Click(object sender, RoutedEventArgs e)
        {
            var tag = (sender as Button).DataContext;
            if (tag != null)
                MainViewModel.Current.Tags.Remove((string)tag);
        }

        private void NewTagTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter && !String.IsNullOrEmpty(NewTagTextBox.Text))
            {
                MainViewModel.Current.Tags.Add(NewTagTextBox.Text);
                NewTagTextBox.Text = String.Empty;
            }
        }

        private async void TagsDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            await MainViewModel.Current.SaveTagsAsync();
            await ReloadQuestions();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await ReloadQuestions();
        }
    }
}
