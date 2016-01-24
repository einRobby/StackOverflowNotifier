using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StackOverflowNotifier.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var path = String.Format("https://stackexchange.com/oauth/dialog?client_id={0}&scope=read_inbox&redirect_uri={1}", App.ClientId, App.RedirectUri);
            WebViewer.Navigate(new Uri(path));
        }

        private async void WebViewer_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            // Check if new location is redirect uri
            if (args.Uri.AbsoluteUri.IndexOf(App.RedirectUri) == 0)
            {
                // Get parameters
                var encoder = new WwwFormUrlDecoder(args.Uri.AbsoluteUri.Replace('#', '&')); // Replacing # with ? to make GetFirstValueByName() work
                var accessToken = encoder.GetFirstValueByName("access_token");
                var expires = encoder.GetFirstValueByName("expires");
                if (!String.IsNullOrEmpty(accessToken))
                {
                    // Success
                    App.AccessToken = accessToken;
                    App.Expires = expires;

                    Frame.GoBack();
                    var successDialog = new MessageDialog("Successfully logged in.", "Success!");
                    await successDialog.ShowAsync();
                    return;
                }

                // Show error message, if something went wrong
                var errorDialog = new MessageDialog("Whoops, we could not log you in successfully. Sorry for that!", "Someting went wrong...");
                await errorDialog.ShowAsync();
                Frame.GoBack();
            }
        }
    }
}
