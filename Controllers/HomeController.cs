using EDUZilla.Data;
using EDUZilla.Models;
using EDUZilla.Services;
using EDUZilla.ViewModels.Announcement;
using EDUZilla.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace EDUZilla.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AnnouncementService _announcementService;
        private readonly ParentService _parentService;

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, UserManager<ApplicationUser> userManager, AnnouncementService announcementService, ParentService parentService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _userManager = userManager;
            _announcementService = announcementService;
            _parentService = parentService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Notice");
            }
            else
                return View();

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(LoginViewModel user)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if(result.Succeeded)
                {
                    return RedirectToAction("Notice");
                }

                ModelState.AddModelError(string.Empty, "Nieudana próba logowania!");
            }

            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Marks()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Notice()
        {
            List<ShowAnnoucementViewModel> list = await _announcementService.GetAnnouncementListAsync();
            List<ShowAnnoucementViewModel> listFiltered = new List<ShowAnnoucementViewModel>();
            var user = await _userManager.GetUserAsync(User);
            foreach (var notice in list)
            {
                if(notice.ChosenClassId != null)
                {
                    var result = await _parentService.CheckIfParentOrStudent((int)notice.ChosenClassId, user.Id);
                    if(result == true)
                    {
                        listFiltered.Add(notice);
                    }
                }
                else
                {
                    listFiltered.Add(notice);
                }
            }
            return View(listFiltered);
        }
        public IActionResult Schedule()
        {
            return View();
        }
        public IActionResult Tests()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult RemindPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RemindPassword(ChangeEmailViewModel model)
        {
            var email = model.NewEmail.ToUpper();
            var user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);            

            await _emailSender.SendEmailAsync(
                model.NewEmail,
                "Change password",
                $"Please change your password by <a href='{"https://localhost:7048/Home/SetNewPassword"}'>clicking here</a>. " +
                $"Also input this token: " + token + " in token field.");


            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SetNewPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SetNewPassword(ChangePasswordViewModel newPassword)
        {
            var email = newPassword.Email.ToUpper();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, newPassword.Token, newPassword.Password);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ReturnToHomePage()
        {
            return RedirectToAction("/Views/Shared/Index");
        }
    }
}