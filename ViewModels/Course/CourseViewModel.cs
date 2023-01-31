using EDUZilla.Models;
using EDUZilla.ViewModels.FileModel;

namespace EDUZilla.ViewModels.Course
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<FileViewModel> Files { get; set; } 
    }
}
