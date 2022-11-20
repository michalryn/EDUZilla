using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDUZilla.Models
{
    [Table("Classes")]
    public class Class
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? TutorId { get; set; }

        [ForeignKey("TutorId")]
        public virtual Teacher? Tutor { get; set; }
        public virtual ICollection<Student>? Students { get; set; }
    }
}
