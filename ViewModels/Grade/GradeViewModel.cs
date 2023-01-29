namespace EDUZilla.ViewModels.Grade
{
    public class GradeViewModel
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }

        public string? CourseName { get; set; }
    }
}
