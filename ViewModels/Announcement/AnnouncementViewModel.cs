using Microsoft.AspNetCore.Mvc.Rendering;
using EDUZilla.ViewModels.Teacher;
using System.ComponentModel.DataAnnotations;

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
        public string Sender { get; set; }

        public SelectListItem? Class { get; set; }


    }
}
