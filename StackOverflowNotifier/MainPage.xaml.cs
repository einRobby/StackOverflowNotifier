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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StackOverflowNotifier
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var stackOverflowRed = (Color)Application.Current.Resources["StackOverflowRed"];

            ApplicationView.GetForCurrentView().TitleBar.ForegroundColor = Colors.White;
            ApplicationView.GetForCurrentView().TitleBar.InactiveForegroundColor = Colors.Wheat;
            ApplicationView.GetForCurrentView().TitleBar.ButtonInactiveForegroundColor = Colors.Wheat;
            ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = stackOverflowRed;
            ApplicationView.GetForCurrentView().TitleBar.InactiveBackgroundColor = stackOverflowRed;
            ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = stackOverflowRed;
            ApplicationView.GetForCurrentView().TitleBar.ButtonInactiveBackgroundColor = stackOverflowRed;


            //var builder = new BackgroundTaskBuilder
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await MainViewModel.Current.LoadAsync();
            await ReloadQuestions();
        }

        private async Task ReloadQuestions()
        {
            ProgressIndicator.Visibility = Visibility.Visible;
            await MainViewModel.Current.LoadQuestionsAsync();
            ProgressIndicator.Visibility = Visibility.Collapsed;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
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
            await MainViewModel.Current.SaveAsync();
            await ReloadQuestions();
        }
    }
}
