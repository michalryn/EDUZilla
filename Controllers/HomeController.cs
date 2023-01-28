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

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, UserManager<ApplicationUser> userManager, AnnouncementService announcementService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _userManager = userManager;
            _announcementService = announcementService;
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
            return View(list);
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
            var callbackUrl = Url.Page(
                "/Views/Home/SetNewPassword",
                pageHandler: null,
                values: new { area = "Identity", model = model },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                model.NewEmail,
                "Reset password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SetNewPassword(ChangeEmailViewModel model)
        {
            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SetNewPassword(ChangeEmailViewModel model, ChangePasswordViewModel newPassword)
        {
            var user = await _userManager.FindByEmailAsync(model.NewEmail);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (user == null || token == null)
            {
                return RedirectToAction("Index");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, token, newPassword.Password);
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