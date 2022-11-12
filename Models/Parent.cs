namespace EDUZilla.Data.Entities
{
    public class Parent : ApplicationUser
    {
        public virtual ICollection<Student> Children { get; set; }
    }
}
