using System;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using Project.Model;
using Project.View;
using Project.ViewModel;

namespace Project.Helpers
{

    /// <summary>
    /// Central listenere for all messages of the app.
    /// </summary>
    public class MessageListener
    {
        #region constructors and destructors

        public MessageListener()
        {
            InitMessenger();
        }

        #endregion

        #region methods

        /// <summary>
        /// Is called by the constructor to define the messages we are interested in.
        /// </summary>
        private void InitMessenger()
        {
            //Announcement of delete element(s)
            Messenger.Default.Register<DeleteMessage>(this, (msg) =>
            {
                var result = MessageBox.Show($"Вы действительно хотите удалить {msg.Notification} из списка?", "Удаление", MessageBoxButton.OKCancel);
                msg.Execute(result);
            });

            //Announcement of errors about unsuccessful Xml load
            Messenger.Default.Register<Exception>(this, (msg) =>
            {
                MessageBox.Show("Во время загрузки файла возникла ошибка: \n" + msg.Message, "Неудачная загрузка",
                    MessageBoxButton.OK);
            });

            Messenger.Default.Register<MessageFromMainToOkCancel>(
                this, msg =>
                {
                    var window = new OkCandelView();
                    if (window.DataContext is OkCancelViewModel model)
                    {
                        switch (msg.UserActionMainWindow)
                        {
                            case UserActionsMainWindow.AddNew:
                                model.Student = new StudentModel();
                                window.Title = "Добавление записи";
                                break;
                            case UserActionsMainWindow.EditExist:
                                model.Student = msg.EditableStudent;
                                window.Title = "Изменение записи";
                                break;
                        }
                        model.UserActionOkCancel = UserActionsOkCancel.None;
                    }
                    window.ShowDialog();
                });

            Messenger.Default.Register<MessageFromOkCancelToMain>(
                this, msg =>
                {
                    var window = new MainView();
                    if (window.DataContext is MainViewModel model)
                    {
                        switch (model.UserActionMainWindow)
                        {
                            case UserActionsMainWindow.AddNew:
                                switch (msg.UserActionOkCancel)
                                {
                                    case UserActionsOkCancel.Ok:
                                        model.CollectionOfStudent.Add(msg.EditedStudent);
                                        break;
                                    case UserActionsOkCancel.Cancel:
                                        Trace.WriteLine("Cancel");
                                        break;
                                }
                                break;
                            case UserActionsMainWindow.EditExist:
                                switch (msg.UserActionOkCancel)
                                {
                                    case UserActionsOkCancel.Ok:
                                        model.CollectionOfStudent[model.SelectedIndex] = msg.EditedStudent;
                                        break;
                                    case UserActionsOkCancel.Cancel:
                                        Trace.WriteLine("Cancel");
                                        break;
                                }
                                break;
                        }
                        model.UserActionMainWindow = UserActionsMainWindow.None;
                    }
                });
        }

        #endregion

        #region properties

        public bool BindableProperty => true;

        #endregion
    }
}
