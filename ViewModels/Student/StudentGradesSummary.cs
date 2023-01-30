namespace EDUZilla.ViewModels.Student
{
    public class StudentGradesSummary
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public List<CourseGradesSummary>? CoursesSummary { get; set; } 
        public double OverallAverege { get; set; }
    }
}
