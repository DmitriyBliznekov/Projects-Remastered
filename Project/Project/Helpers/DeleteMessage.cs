using System;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace Project.Helpers
{
    public class DeleteMessage : NotificationMessageAction<MessageBoxResult>
    {
        public DeleteMessage(object sender, string notification, Action<MessageBoxResult> callback) :
            base(sender, notification, callback)
        {
        }
    }
}