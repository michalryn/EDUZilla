using Microsoft.AspNetCore.Identity;

namespace EDUZilla.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
