using FrontToBack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class EmailConfirmationController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public EmailConfirmationController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            AppUser user= await _userManager.FindByEmailAsync(email);
            var result = await _userManager.ConfirmEmailAsync(user, token);
           
            return View(result.Succeeded?"ConfirmEmail":"Error");
        }
    }
}
