using System.ComponentModel.DataAnnotations;

namespace EDUZilla.ViewModels.User
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Podaj imię użytkownika!")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Podaj nazwisko użytkownia!")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Podaj adres e-mail!")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Podaj hasło!")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Wybierz typ użytkownika")]
        public string Role { get; set; }
        public string[]? Roles { get; set; }
    }
}
