using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.ViewModel;

namespace Project.Helpers
{
    class OpenChildFormWithNewElement
    {
        public OpenChildFormWithNewElement(StudentModel student, bool newStudent)
        {
            Student = student;
            NewStudent = newStudent;
        }

        public StudentModel Student { get; private set; }

        public bool NewStudent { get; set; }

    }
}