using System.ComponentModel.DataAnnotations;

namespace EDUZilla.ViewModels.User
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "FirstNameError")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastNameError")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "EmailError")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordError")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "RoleError")]
        [Display(Name = "Role")]
        public string Role { get; set; }
        public string[]? Roles { get; set; }
    }
}
