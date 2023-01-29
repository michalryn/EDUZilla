namespace EDUZilla.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? StudentId { get; set; }
        public virtual Student? Student { get; set; }
        public int? CourseId { get; set; }
        public virtual Course? Course { get; set; }
    }
}
