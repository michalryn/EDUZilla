namespace EDUZilla.Data.Entities
{
    public class Student : ApplicationUser
    {
        public int Grade { get; set; }

        public virtual Parent Parent { get; set; }
    }
}
