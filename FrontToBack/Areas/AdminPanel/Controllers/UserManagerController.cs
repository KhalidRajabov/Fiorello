﻿using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class UserManagerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserManagerController
            (UserManager<AppUser> usermanager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager
            )
        {
            _userManager = usermanager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index(string search)
        {
            var users = search == null ? 
                _userManager.Users.ToList() :
                _userManager.Users.Where(users => users.FullName.ToLower().Contains(search.ToLower())||
                users.UserName.ToLower().Contains(search.ToLower())||
                users.Email.ToLower().Contains(search.ToLower())).ToList();
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            AppUser user = await _userManager.FindByIdAsync(id);
            if(user==null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            var dbRoles = _roleManager.Roles.ToList();
            RoleVM rolevm = new RoleVM
            {
                FullName = user.FullName,
                Roles = dbRoles,
                UserRoles = userRoles,
                UserId = user.Id
            };
            return View(rolevm);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(List<string> roles, string id)
        {
            if (id == null) return NotFound();
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);
            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);
            return RedirectToAction("index");
        }

        /*public async Task<IActionResult> Index()
        {
            List<AppUser> users = _userManager.Users.ToList();
            List<UserVM> usersVM = new List<UserVM>();
            foreach (AppUser user in users)
            {
                UserVM userVM = new UserVM
                {
                    Id = user.Id,
                    Name = user.FullName,
                    Username = user.UserName,
                    Email = user.Email,
                    IsActivated = user.IsActivated,
                    Role = (await _userManager.GetRolesAsync(user))[0]
                };
            usersVM.Add(userVM);

            }
            return View(usersVM);
        }

        public async Task<IActionResult> Activate(string? id)
        {
            if (id == null) return NotFound();
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }*/
    }
}
