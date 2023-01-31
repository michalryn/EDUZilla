using System.ComponentModel.DataAnnotations;

namespace EDUZilla.ViewModels.Course
{
    public class AddFileForm
    {
        public int CourseId { get; set; }
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string FileDescription { get; set; }
    }
}
