using System.ComponentModel.DataAnnotations;

namespace EDUZilla.Models
{
    public class Course
    {
        public Course()
        {
            this.Teachers = new HashSet<Teacher>();
            this.Classes = new HashSet<Class>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Teacher>? Teachers { get; set; }
        public virtual ICollection<Class>? Classes { get; set; }
    }
}
