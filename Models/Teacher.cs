using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZilla.Models
{
    public class Teacher : ApplicationUser
    {
        public Teacher()
        {
            this.Courses = new HashSet<Course>();
        }
        public virtual Class? TutorClass { get; set; }
        public virtual ICollection<Course>? Courses { get; set; }
    }
}
