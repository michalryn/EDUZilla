using EDUZilla.ViewModels.Grade;

namespace EDUZilla.ViewModels.Student
{
    public class CourseGrades
    {
        public string CourseName { get; set; }
        public List<GradeViewModel>? Grades { get; set; }
    }
}
