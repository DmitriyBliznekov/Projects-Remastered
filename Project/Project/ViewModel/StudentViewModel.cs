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
    public class StudentViewModel : ViewModelBase
    {
        public StudentViewModel()
        {
            OkCommand = new RelayCommand<Window>(OnOk, CanOk);
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

        public bool NewOrEdit { get; set; }

        public string SelectedIndex { get; set; }

        public string FirstNameSave { get; set; }
        public string LastNameSave { get; set; }
        public string AgeSave { get; set; }
        public string GenderSave { get; set; }


        public RelayCommand<Window> OkCommand { get; private set; }
        public RelayCommand<Window> CloseWindowCommand { get; private set; }

        private void OnOk(Window window)
        {
            if (window != null)
            {
                MessengerInstance.Send(new BackDataFromChildForm(Student, NewOrEdit));
                window.Close();
            }
        }

        private bool CanOk(Window window)
        {
            return true;
        }

        private void OnCloseWindow(Window window)
        {
            if (window != null)
            {
                MessengerInstance.Send(
                    new CancelAndRemove(new StudentModel()
                    { FirstName = FirstNameSave, LastName = LastNameSave, Age = AgeSave, Gender = GenderSave },
                    SelectedIndex, NewOrEdit));
                window.Close();
            }
        }
    }
}
