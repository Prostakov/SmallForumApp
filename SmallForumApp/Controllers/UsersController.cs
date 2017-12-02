using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmallForumApp.Models;

namespace SmallForumApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Users = _userManager.Users.Include(u => u.Roles).ToList();
            foreach (ApplicationUser user in ViewBag.Users)
            {
                if (user.Roles.Any())
                {
                    var role = await _roleManager.FindByIdAsync(user.Roles.First().RoleId);
                    user.Role = role.Name;
                }
            }
            return View();
        }
    }
}