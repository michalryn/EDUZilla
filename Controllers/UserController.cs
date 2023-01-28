using EDUZilla.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using EDUZilla.ViewModels.User;
using Microsoft.AspNetCore.Authorization;

namespace EDUZilla.Controllers
{
    public class UserController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Email()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return View();
                }
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordViewModel.OldPassword, changePasswordViewModel.Password);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                await _signInManager.RefreshSignInAsync(user);


            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Password()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Password(String w)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View();
            }

            await _emailSender.SendEmailAsync(
                user.Email,
                "Change password",
                $"Please change your password by <a href='{"https://localhost:7048/User/ChangePassword"}'>clicking here</a>.");


            return View();
        }
        [HttpGet]
        public IActionResult ChangeEmail()

        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ChangeEmail(ChangeEmailViewModel changeEmailViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return View();
                }
                if (changeEmailViewModel.NewEmail != user.Email)
                {
                    var code = await _userManager.GenerateChangeEmailTokenAsync(user, changeEmailViewModel.NewEmail);
                    var changeEmailResult = await _userManager.ChangeEmailAsync(user, changeEmailViewModel.NewEmail, code);
                    if (!changeEmailResult.Succeeded)
                    {
                        foreach (var error in changeEmailResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    await _signInManager.RefreshSignInAsync(user);


                    return RedirectToAction("Index");
                }


            }
            return View();
        }
        


    }
}
