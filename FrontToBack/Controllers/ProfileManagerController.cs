using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class ProfileManagerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ProfileManagerController
            (UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            UserInfoVM userVM = new UserInfoVM();
            userVM.Id=user.Id;
            userVM.Fullname = user.FullName;
            userVM.Email = user.Email;
            userVM.About = user.About;
            userVM.Username = user.UserName;
            userVM.Phone = user.PhoneNumber;
            userVM.ImageURL = user.ImageURL;
            return View(userVM);
        }


        public async Task<IActionResult> ChangePassword(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, string NewPassword)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return RedirectToAction("index", "home");
        }
    }
}
