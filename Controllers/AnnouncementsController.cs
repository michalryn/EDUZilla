using EDUZilla.Models;
using EDUZilla.Services;
using EDUZilla.ViewModels.Announcement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EDUZilla.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly AnnouncementService _announcementService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AnnouncementsController(IEmailSender emailSender, AnnouncementService announcementService, UserManager<ApplicationUser> userManager)
        {
            _emailSender = emailSender;
            _announcementService = announcementService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> AddNewAnnouncement()
        {


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNewAnnouncement(AnnouncementViewModel announcementViewModel)
        {
            var result = await _announcementService.AddAnnouncementAsync(announcementViewModel);
            if (result == false)
            {
                return View();
            }
            return RedirectToAction("/Views/Home/Notice");
        }

    }
}
