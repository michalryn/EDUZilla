using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using EDUZilla.LanguageResources;

namespace EDUZilla.ViewModels.User
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "EmailAddress")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
