using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;

namespace Project.Helpers
{
    public class CancelAndRemove 
    {
        public CancelAndRemove(StudentModel student, string selectedIndex, bool newOrEdit)
        {
            Student = student;
            SelectedIndex = selectedIndex;
            NewOrEdit = newOrEdit;
        }

        public StudentModel Student { get; set; }

        public string SelectedIndex { get; set; }

        public bool NewOrEdit { get; set; }
    }
}
