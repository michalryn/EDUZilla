using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace EDUZilla.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult Change(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) }
                );

            return RedirectToAction("Index", "Home");
        }
    }
}
