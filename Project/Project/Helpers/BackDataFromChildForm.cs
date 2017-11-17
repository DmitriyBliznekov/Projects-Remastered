using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.View;
using Project.ViewModel;

namespace Project.Helpers
{
    public class BackDataFromChildForm
    {
        public BackDataFromChildForm(StudentModel student, bool newOrEdit)
        {
            Student = student;
            NewOrEdit = newOrEdit;
        }

        public StudentModel Student { get; private set; }

        public bool NewOrEdit { get; set; }
    }
}
