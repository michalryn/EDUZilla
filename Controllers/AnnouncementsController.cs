using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EDUZilla.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly IEmailSender _emailSender;
        public AnnouncementsController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult AddNewAnnouncement()
        {
            return View();
        }

    }
}
