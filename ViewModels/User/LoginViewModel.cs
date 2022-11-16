using System.ComponentModel.DataAnnotations;

namespace EDUZilla.ViewModels.User
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Adres email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Pamietaj mnie")]
        public bool RememberMe { get; set; }
    }
}
