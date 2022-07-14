﻿using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace FrontToBack.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _usermanager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController
            (UserManager<AppUser> usermanager, 
            RoleManager<IdentityRole>roleManager, 
            SignInManager<AppUser> signInManager
            )
        {
            _usermanager = usermanager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)   return RedirectToAction("index", "home");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("index", "home");

            if (!ModelState.IsValid) return View();
            AppUser appUser = new AppUser
            {
                FullName = registerVM.Fullname,
                UserName = registerVM.Username,
                Email = registerVM.Email

            };
            IdentityResult result = await _usermanager.CreateAsync(appUser, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
                return View(registerVM);
            }
            await _usermanager.AddToRoleAsync(appUser, "Member");
            
            return RedirectToAction("login", "account");
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)    return RedirectToAction("index", "home");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginvm, string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("index", "home");
            if (!ModelState.IsValid) return View();
            AppUser appUser = await _usermanager.FindByEmailAsync(loginvm.Email);
            if (appUser == null) 
            {
                ModelState.AddModelError("", "Email or password is wrong"); 
                return View(loginvm); 
            }

            SignInResult result = await _signInManager
                .PasswordSignInAsync(appUser, loginvm.Password, loginvm.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is blocked");
                return View(loginvm);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(loginvm);
            }
            if (ReturnUrl!=null)
            {
                return Redirect(ReturnUrl);
            }

            await _signInManager.SignInAsync(appUser, isPersistent: true);
            var roles = (await _usermanager.GetRolesAsync(appUser))[0];
            
            if (roles.ToLower()=="admin"|| roles.ToLower() == "superadmin")
            {

                return RedirectToAction("index", "dashboard", new { Area = "AdminPanel" });
            }
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        /*public async Task CreateRole()
        {

            *//*-------When needed a new role, uncomment this method and -------
            -------add a new role, then go to action in browser------ -*//*




            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }
            if (!await _roleManager.RoleExistsAsync("Member"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
            }
            if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
            }
        }*/
    }
}
