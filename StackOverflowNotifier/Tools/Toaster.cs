using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace StackOverflowNotifier.Tools
{
    public static class Toaster
    {
        public static void ShowSimpleToastNotification(string text)
        {
            // template to load for showing Toast Notification
            var xmlToastTemplate = "<toast launch=\"app-defined-string\">" +
                                     "<visual>" +
                                       "<binding template =\"ToastGeneric\">" +
                                         "<text>Stack Overflow</text>" +
                                         "<text>" +
                                           text +
                                         "</text>" +
                                       "</binding>" +
                                     "</visual>" +
                                   "</toast>";

            // load the template as XML document
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlToastTemplate);

            // create the toast notification and show to user
            var toastNotification = new ToastNotification(xmlDocument);
            var notification = ToastNotificationManager.CreateToastNotifier();
            notification.Show(toastNotification);
        }
    }
}
