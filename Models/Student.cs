namespace EDUZilla.Models
{
    public class Student : ApplicationUser
    {
        public int Grade { get; set; }
        public string? ParentId { get; set; }
        public virtual Parent? Parent { get; set; }
        public int? ClassId { get; set; }
        public virtual Class? Class { get; set; }
    }
}
