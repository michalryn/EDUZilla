using Microsoft.AspNetCore.Mvc.Rendering;
using EDUZilla.ViewModels.Teacher;
using System.ComponentModel.DataAnnotations;
using EDUZilla.ViewModels.Class;

namespace EDUZilla.ViewModels.Announcement
{
    public class AnnouncementViewModel
    {
        public int? AnnouncementId { get; set; }
        [Required]
        [Display(Name = "Topic")]
        public string Topic { get; set; }
        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public string? SenderId { get; set; }
        public int? ChosenClassId { get; set; }
        public ClassListViewModel? ChosenClass { get; set; }



    }
}
