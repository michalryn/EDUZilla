using EDUZilla.Models;
using EDUZilla.Services;
using EDUZilla.ViewModels.Announcement;
using EDUZilla.ViewModels.Class;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ParentService _parentService;
        public AnnouncementsController(IEmailSender emailSender, AnnouncementService announcementService, UserManager<ApplicationUser> userManager, ParentService parentService)
        {
            _emailSender = emailSender;
            _announcementService = announcementService;
            _userManager = userManager;
            _parentService = parentService;
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<IActionResult> AddNewAnnouncement()
        {
            var group = await _announcementService.GetClassesAsync(_userManager.GetUserAsync(User).Result.Id);
            if (group == null)
            {
                return View();

            }
            AnnouncementViewModel announcementViewModel = new AnnouncementViewModel()
            {
                ChosenClass = new ClassListViewModel()
                {
                    Id = group.Id,
                    Name = group.Name
                }
            };
            return View(announcementViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewAnnouncement(AnnouncementViewModel announcementViewModel)
        {
            announcementViewModel.Created = DateTime.Now;
            announcementViewModel.SenderId = _userManager.GetUserAsync(User).Result.Id;
            var result = await _announcementService.AddAnnouncementAsync(announcementViewModel);
            if (result == false)
            {
                return View();
            }
            if (announcementViewModel.ChosenClassId != null)
            {
                var parents = await _parentService.GetParents((int)announcementViewModel.ChosenClassId);
                foreach (var parent in parents)
                {
                    await _emailSender.SendEmailAsync(
                      parent.Email,
                     "New annoucement: " + announcementViewModel.Topic,
                     "" + announcementViewModel.Content);
                }


            }

            return Redirect("/Home/Notice");
        }

    }
}
