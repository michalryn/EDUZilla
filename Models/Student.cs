namespace EDUZilla.Models
{
    public class Student : ApplicationUser
    {
        public int Grade { get; set; }

        public virtual Parent Parent { get; set; }
    }
}
