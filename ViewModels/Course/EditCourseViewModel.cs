using EDUZilla.ViewModels.Class;
using EDUZilla.ViewModels.Teacher;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EDUZilla.ViewModels.Course
{
    public class EditCourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<TeacherListViewModel>? Teachers { get; set; }
        public List<SelectListItem>? AvailableTeachers { get; set; }
        public IList<ClassListViewModel>? Classes { get; set; }
        public IList<SelectListItem>? AvailableClasses { get; set; }
    }
}
