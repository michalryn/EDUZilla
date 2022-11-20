namespace EDUZilla.Models
{
    public class Teacher : ApplicationUser
    {
        public int? TutorClassId { get; set; }
        public virtual Class? TutorClass { get; set; }
    }
}
