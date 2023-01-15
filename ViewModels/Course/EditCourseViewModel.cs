using EDUZilla.ViewModels.Teacher;

namespace EDUZilla.ViewModels.Course
{
    public class EditCourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<TeacherListViewModel>? Teachers { get; set; }
    }
}
