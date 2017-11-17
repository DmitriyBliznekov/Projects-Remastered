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

            //if New clicked
            Messenger.Default.Register<OpenChildFormWithNewElement>(
                this,
                msg =>
                {
                    var window = new StudentView();
                    var model = window.DataContext as StudentViewModel;
                    if (model != null)
                    {
                        model.Student = msg.Student;
                        model.NewOrEdit = msg.NewStudent;
                    }
                    window.ShowDialog();
                });

            //if edit clicked
            Messenger.Default.Register<OpenChildWindowAddOrEdit>(
                this,
                msg =>
                {
                    var window = new StudentView();
                    var model = window.DataContext as StudentViewModel;
                    if (model != null)
                    {
                        model.Student = msg.Student;
                        model.NewOrEdit = msg.Edit;

                        model.FirstNameSave = msg.FirstName;
                        model.LastNameSave = msg.LastName;
                        model.AgeSave = msg.Age;
                        model.GenderSave = msg.Gender;

                        model.SelectedIndex = msg.Index;
                    }
                    window.ShowDialog();
                });
            //data from child window
            Messenger.Default.Register<BackDataFromChildForm>(
                this,
                msg =>
                {
                    var window = new MainWindow();
                    var model = window.DataContext as MainViewModel;
                    if (model != null)
                    {
                        if (msg.NewOrEdit)
                            model.CollectionOfStudent.Add(msg.Student);

                        //if edit mode
                        else
                        {
                            //model.CollectionOfStudent[msg.Index] = msg.Student;
                        }
                    }
                });
            //if cancel clicked
            Messenger.Default.Register<CancelAndRemove>(
                this,
                msg =>
                {
                    var window = new MainWindow();
                    var model = window.DataContext as MainViewModel;
                    if (model != null)
                    {
                        //Trace.WriteLine("Cancel");
                        //model.CollectionOfStudent[msg.]
                        if (!msg.NewOrEdit)
                            model.CollectionOfStudent[int.Parse(msg.SelectedIndex)] = msg.Student;
                    }
                });
        }

        #endregion

        #region properties

        public bool BindableProperty => true;

        #endregion
    }
}
