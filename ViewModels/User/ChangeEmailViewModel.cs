using System.ComponentModel.DataAnnotations;

namespace EDUZilla.ViewModels.User
{
    public class ChangeEmailViewModel
    {
        [Required(ErrorMessage = "EmailError")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string NewEmail { get; set; }
    }
}
