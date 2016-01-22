using Newtonsoft.Json;
using StackOverflowNotifier.Models;
using StackOverflowNotifier.Tools;
using StackOverflowNotifier.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace StackOverflowNotifier
{
    public class BackgroundWorker : IBackgroundTask
    {
        public object Toster { get; private set; }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Load new questions
            await MainViewModel.Current.LoadAsync();

            // Show Notification if needed
            if (MainViewModel.Current.NewQuestionCount > 0)
            {                
                Toaster.ShowSimpleToastNotification($"{MainViewModel.Current.NewQuestionCount} new unanswered questions for your tags.");
            }
        }
    }
}
