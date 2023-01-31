using EDUZilla.ViewModels.Student;

namespace EDUZilla.ViewModels.Class
{
    public class ClassGradesSummaryVM
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public List<StudentGradesSummaryVM> Students { get; set; }
    }
}
