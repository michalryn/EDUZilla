using System.ComponentModel.DataAnnotations;

namespace EDUZilla.ViewModels.Role
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "RoleNameError")]
        [Display(Name = "RoleName")]
        public string RoleName { get; set; }
    }
}
