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
            await MainViewModel.Current.LoadTagsAsync();

            // Load new questions
            await MainViewModel.Current.LoadQuestionsAsync();

            // Show Notification if needed
            if (MainViewModel.Current.NewQuestionCount > 0)
            {                
                // Delete all other notifications
                NotificationHelper.DeleteAllNotifications();

                // Check if 'question' needs to be plural
                var plural = "s";
                if (MainViewModel.Current.NewQuestionCount > 1)
                    plural = "";

                // Show notification
                NotificationHelper.ShowSimpleToastNotification("Stack Overflow", $"{MainViewModel.Current.NewQuestionCount} new unanswered question{plural}.");

                // Update tile
                NotificationHelper.UpdateBadgeCounter(MainViewModel.Current.NewQuestionCount);
            }

            _deferral.Complete();
        }
    }
}
