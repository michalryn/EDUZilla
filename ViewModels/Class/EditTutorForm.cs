using Microsoft.AspNetCore.Mvc.Rendering;

namespace EDUZilla.ViewModels.Class
{
    public class EditTutorForm
    {
        public int ClassId { get; set; }
        public SelectListItem Teacher { get; set; }
    }
}
