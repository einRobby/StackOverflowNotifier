using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace StackOverflowNotifier.UWP.Shared.Tools
{
    public static class BackgroundHelper
    {
        public static async void RegisterBackgroundTask(uint minuteInterval)
        {
            var taskName = "UnreadNotifierTask";

            // Unrgister task if interval is 0
            if (minuteInterval == 0)
            {
                var registeredTask = BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == taskName);
                if (registeredTask.Value != null)
                    registeredTask.Value.Unregister(false);
                return;
            }

            // Ensure that interval is at least 15 minutes
            if (minuteInterval < 15)
                minuteInterval = 15;

            // Check if task has already been registered
            if (BackgroundTaskRegistration.AllTasks.Any(t => t.Value.Name == taskName))
            {
                // Task as already been registered. 
                // Check if task needs to be re-registered. This is the case if the notification interval has changed.
                var currentInterval = Windows.Storage.ApplicationData.Current.LocalSettings.Values["currentInterval"];
                if (currentInterval != null && Convert.ToUInt32(currentInterval) != minuteInterval)
                {
                    // Current notification interval differs from the new one. Unregister the current task
                    var registeredTask = BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == taskName);
                    registeredTask.Value.Unregister(false);
                }
                else
                {
                    // Task has been found but does not need to get chaneged. Keep everything as it is
                    return;
                }
            }

            // Register background task
            await BackgroundExecutionManager.RequestAccessAsync();
            var builder = new BackgroundTaskBuilder();
            builder.Name = taskName;
            builder.TaskEntryPoint = "StackOverflowNotifier.UWP.BackgroundTask.UnreadNotifierTask";
            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            builder.SetTrigger(new TimeTrigger(minuteInterval, false));
            builder.Register();
        }
    }
}
