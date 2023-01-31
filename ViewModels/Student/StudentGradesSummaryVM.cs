using EDUZilla.ViewModels.Grade;

namespace EDUZilla.ViewModels.Student
{
    public class StudentGradesSummaryVM
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<GradeViewModel>? Grades { get; set; }
        public double GradeAverage { get; set; }
    }
}
