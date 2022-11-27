using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZilla.Models
{
    public class Teacher : ApplicationUser
    {
        public virtual Class? TutorClass { get; set; }
    }
}
