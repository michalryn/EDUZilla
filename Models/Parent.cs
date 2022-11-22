namespace EDUZilla.Models
{
    public class Parent : ApplicationUser
    {
        public virtual ICollection<Student> Children { get;} = new List<Student>();
    }
}
