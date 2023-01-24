using EDUZilla.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

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
        public IActionResult Password()
        {
            return View();
        }



        [HttpPost]
        public async Task<ActionResult> RemindPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View();
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            if(code == null)
            {
                return View();
            }
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                user.Email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return View();

        }

        public async Task<ActionResult> ChangePassword(string OldPassword,string NewPassword)
        {
            var user = await _userManager.GetUserAsync(User);               
            if (user == null)
            {
                return View();
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
            await _signInManager.RefreshSignInAsync(user);

            return View();
        }
    }
}
