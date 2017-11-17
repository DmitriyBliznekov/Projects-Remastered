using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Project.Helpers;
using Project.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Xml;
using System.Xml.Serialization;

namespace Project.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            CollectionOfStudent = new ObservableCollection<StudentModel>();
            StudentsView = CollectionViewSource.GetDefaultView(CollectionOfStudent) as ListCollectionView;

            SelectedIndex = -1;

            AddCommand = new RelayCommand(OnAdd, () => true);
            DeleteCommand = new RelayCommand<object>(OnDelete, (o) => SelectedStudent != null);
            LoadCommand = new RelayCommand(OnLoad, () => true);
            SaveCommand = new RelayCommand(OnSave, () => true);
            ClearCommand = new RelayCommand(OnClear, () => CollectionOfStudent.Any());
            EditCommand = new RelayCommand(OnEdit, () => SelectedStudent != null);
            StudentsView = CollectionViewSource.GetDefaultView(CollectionOfStudent) as ListCollectionView;

            StudentsView.CurrentChanged += (s, e) =>
            {
                RaisePropertyChanged(nameof(StudentModel));
            };
        }

        #region Fields
        private StudentModel selectedStudent;
        private int selectedIndex;
        #endregion

        #region Properties
        public ObservableCollection<StudentModel> CollectionOfStudent { get; set; }

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand<object> DeleteCommand { get; private set; }
        public RelayCommand LoadCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                RaisePropertyChanged(nameof(SelectedIndex));
            }
        }

        public StudentModel SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                selectedStudent = value;
                DeleteCommand.RaiseCanExecuteChanged(); // Activate Delete button after selecting an item
                EditCommand.RaiseCanExecuteChanged(); // Activate Edit button after selecting an item
                ClearCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(SelectedStudent));
            }
        }

        public StudentModel StudentModel
        {
            get => StudentsView.CurrentItem as StudentModel;
            set
            {
                StudentsView.MoveCurrentTo(value);
                RaisePropertyChanged(nameof(StudentModel));
            }
        }

        public ListCollectionView StudentsView { get; }
        #endregion

        #region Methods
        private void LoadXmlData(OpenFileDialog ofd, string path)
        {
            path = ofd.FileName;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(path);
                XmlElement root = doc.DocumentElement;
                XmlNodeList nodes = root.SelectNodes("Student");
                foreach (XmlNode node in nodes)
                {
                    CollectionOfStudent.Add(new StudentModel()
                    {
                        FirstName = node["FirstName"].InnerText,
                        LastName = node["Last"].InnerText,
                        Age = node["Age"].InnerText,
                        Gender = node["Gender"].InnerText
                    });
                }

                SubscribeAllCollectionFromFile();
            }
            catch (Exception e)
            {
                MessengerInstance.Send(e);
            }
        }

        private void SubscribeAllCollectionFromFile()
        {
            foreach (var item in CollectionOfStudent)
            {
                item.PropertyChanged += StudentsOnPropertyChanged;
            }
            CollectionOfStudent.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (INotifyPropertyChanged added in e.NewItems)
                    {
                        added.PropertyChanged += StudentsOnPropertyChanged;
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (INotifyPropertyChanged removed in e.OldItems)
                    {
                        removed.PropertyChanged -= StudentsOnPropertyChanged;
                    }
                }
            };
        }

        private void OnAdd()
        {
            //true = new, false = edit
            MessengerInstance.Send(new OpenChildFormWithNewElement(new StudentModel(), true));

            ClearCommand.RaiseCanExecuteChanged();
        }

        private void OnDelete(object students)
        {
            IList listOfSelectedStudents = (IList)students; // Collection of selected students
            var collection = listOfSelectedStudents.Cast<StudentModel>(); // Cast SelectedItemCollection to Collection<Student>

            string namesOfSelectedStudents = string.Empty; // String of all selected students, which are ready to be removed

            var last = collection.ToList().Last(); // The last element in selected collection

            foreach (var item in collection.ToList())
            {
                if (item != last)
                    namesOfSelectedStudents += item.FullName + ", ";
                else
                    namesOfSelectedStudents += item.FullName;
            }

            //Merge message items that can be deleted
            var msg = new DeleteMessage(this, namesOfSelectedStudents, (result) =>
            {
                var test = collection.ToList();
                if (result == MessageBoxResult.OK)
                {
                    test.ForEach(
                        prop =>
                        {
                            CollectionOfStudent.Remove(prop);
                        });

                    foreach (var item in collection.ToList())
                        CollectionOfStudent.Remove(item);

                    ClearCommand.RaiseCanExecuteChanged();
                }       
            });

            MessengerInstance.Send(msg);
        }

        private void OnLoad()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Xml files (*.xml)|*.xml",
                InitialDirectory = Path.Combine(Directory.GetCurrentDirectory() + "\\XmlFiles")
            };

            if (openFileDialog.ShowDialog() == true)
            {
                CollectionOfStudent.Clear();
                LoadXmlData(openFileDialog, openFileDialog.FileName);
                ClearCommand.RaiseCanExecuteChanged(); // Activate Clear button
            }
        }

        private void OnSave()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = "ListOfStudents",
                DefaultExt = ".xml",
                Filter = "Xml files (.xml)|*.xml",
                InitialDirectory = Path.Combine(Directory.GetCurrentDirectory() + "\\XmlFiles")
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                XmlSerialization.SerializeObjectToXml(CollectionOfStudent, saveFileDialog.FileName);
            }
        }

        private void OnClear()
        {
            var msg = new DeleteMessage(this, "удалить все элементы", (result) =>
            {
                if (result == MessageBoxResult.OK)
                    CollectionOfStudent.Clear();

                ClearCommand.RaiseCanExecuteChanged();
            });

            Messenger.Default.Send(msg);
        }

        private void OnEdit()
        {
            //true = new, false = edit
            MessengerInstance.Send(new OpenChildWindowAddOrEdit(SelectedStudent, false, SelectedIndex.ToString(), 
                SelectedStudent.FirstName, SelectedStudent.LastName, SelectedStudent.Age, SelectedStudent.Gender));
        }

        /// <summary>
        /// Event handler for property changes on elements of <see cref="Students"/>.
        /// </summary>
        /// <param name="sender">The student model.</param>
        /// <param name="e">The event arguments.</param>
        private void StudentsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StudentModel.HasErrors) || e.PropertyName == nameof(StudentModel.IsOkay))
            {
                return;
            }
            if (StudentsView.IsEditingItem || StudentsView.IsAddingNew)
            {
                return;
            }
            StudentsView.Refresh();
        }
        #endregion
    }
}