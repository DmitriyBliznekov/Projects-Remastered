using Project.Model;

namespace Project.Helpers
{
    public class OpenChildWindowAddOrEdit
    {
        public OpenChildWindowAddOrEdit(StudentModel student, bool notEdit, string index, string firstName, string lastName, string age, string gender)
        {
            Student = student;
            Edit = notEdit;
            Index = index;

            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Gender = gender;
        }

        public StudentModel Student { get; private set; }

        public bool Edit { get; set; }

        public string Index { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Age { get; set; }

        public string Gender { get; set; }

    }
}