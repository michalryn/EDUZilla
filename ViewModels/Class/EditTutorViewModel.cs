using EDUZilla.ViewModels.Teacher;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EDUZilla.ViewModels.Class
{
    public class EditTutorViewModel
    {
        public int ClassId { get; set; }

        [Display(Name = "ClassName")]
        public string ClassName { get; set; }
        public string? TutorId { get; set; }
        [Display(Name = "TutorName")]
        public string? TutorName { get; set; }
        public SelectListItem? SelectedTeacher { get; set; } 
        [Display(Name = "ChooseTutor")]
        public List<SelectListItem> AvailableTeachers { get; set; }
    }
}
