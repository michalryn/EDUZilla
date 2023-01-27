using EDUZilla.ViewModels.Teacher;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace EDUZilla.ViewModels.Announcement
{
    public class AnnouncementListForm
    {
        public int? AnnouncementId { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public TeacherListViewModel? Sender { get; set; }
        public SelectListItem? SelectedClass { get; set; }

    }
}
