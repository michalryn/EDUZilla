using System.ComponentModel.DataAnnotations;

namespace EDUZilla.ViewModels.Role
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Podaj nazwę roli!")]
        [Display(Name = "Nazwa roli")]
        public string RoleName { get; set; }
    }
}
