using EDUZilla.Models;
using EDUZilla.ViewModels.Student;

namespace EDUZilla.ViewModels.Class
{
    public class EditClassViewModel
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
        public IList<StudentListViewModel> AssignedStudents { get; set; }
        public IList<StudentListViewModel> NotAssignedStudents { get; set; }

    }
}
