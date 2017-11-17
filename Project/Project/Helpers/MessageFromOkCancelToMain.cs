using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.ViewModel;
using Project.Model;

namespace Project.Helpers
{
    public class MessageFromOkCancelToMain
    {
        public MessageFromOkCancelToMain(OkCancelViewModel okCancel)
        {
            UserActionOkCancel = okCancel.UserActionOkCancel;
            EditedStudent = okCancel.Student;
        }

        public UserActionsOkCancel UserActionOkCancel { get; set; }

        public StudentModel EditedStudent { get; set; }
    }
}
