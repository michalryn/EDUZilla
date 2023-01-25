namespace EDUZilla.Models
{
    public class Student : ApplicationUser
    {
        public string? ParentId { get; set; }
        public virtual Parent? Parent { get; set; }
        public int? ClassId { get; set; }
        public virtual Class? Class { get; set; }
        public virtual ICollection<Grade>? Grades { get; set; }
    }
}
