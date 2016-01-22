using StackOverflowNotifier.UWP.Shared.Tools;
using StackOverflowNotifier.UWP.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace StackOverflowNotifier.UWP.BackgroundTask
{
    public sealed class UnreadNotifierTask : IBackgroundTask
    {        
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Initialize
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();
            MainViewModel.Current = new MainViewModel();

            // Load new questions
            await MainViewModel.Current.LoadAsync();

            // Show Notification if needed
            if (MainViewModel.Current.NewQuestionCount > 0)
            {
                Toaster.ShowSimpleToastNotification($"{MainViewModel.Current.NewQuestionCount} new unanswered questions for your tags.");
            }
            else
            {
                Toaster.ShowSimpleToastNotification("No new unanswered questions found for your tags.");
            }

            _deferral.Complete();
        }
    }
}
