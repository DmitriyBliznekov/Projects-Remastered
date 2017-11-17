using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Project.Helpers;
using Project.Model;

namespace Project.ViewModel
{
    public class OkCancelViewModel : ViewModelBase
    {
        public OkCancelViewModel()
        {
            UserActionOkCancel = UserActionsOkCancel.None;
            OkCommand = new RelayCommand<Window>(OnOk, (window) => true);
            CloseWindowCommand = new RelayCommand<Window>(OnCloseWindow);
        }

        private StudentModel student;
        public StudentModel Student
        {
            get => student;
            set
            {
                student = value;
                RaisePropertyChanged(nameof(Student));
            }
        }

        public UserActionsOkCancel UserActionOkCancel { get; set; }
        public RelayCommand<Window> OkCommand { get; private set; }
        public RelayCommand<Window> CloseWindowCommand { get; private set; }

        private void OnOk(Window window)
        {
            if (window == null) return;
            UserActionOkCancel = UserActionsOkCancel.Ok;
            MessengerInstance.Send(new MessageFromOkCancelToMain(this));
            window.Close();
        }

        private void OnCloseWindow(Window window)
        {
            if (window == null) return;
            UserActionOkCancel = UserActionsOkCancel.Cancel;
            MessengerInstance.Send(new MessageFromOkCancelToMain(this));
            window.Close();
        }
    }
}
