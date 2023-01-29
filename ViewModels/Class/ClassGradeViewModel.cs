using EDUZilla.ViewModels.Student;

namespace EDUZilla.ViewModels.Class
{
    public class ClassGradeViewModel
    {
        public int CourseId { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public List<StudentCourseGradesViewModel>? Students { get; set; }
    }
}
