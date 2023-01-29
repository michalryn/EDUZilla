namespace EDUZilla.ViewModels.Grade
{
    public class AddGradeForm
    {
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public int ClassId { get; set; }
        public int GradeValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
    }
}
