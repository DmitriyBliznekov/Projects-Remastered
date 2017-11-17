using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using Project.ViewModel;

namespace Project.Helpers
{
    public class MessageFromMainToOkCancel
    {
        public MessageFromMainToOkCancel(MainViewModel main)
        {
            UserActionMainWindow = main.UserActionMainWindow;
            EditableStudent = main.StudentBeforeChange;
        }

        public UserActionsMainWindow UserActionMainWindow { get; set; }

        public StudentModel EditableStudent { get; set; }
    }
}
