using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TestAuthorizationApp.Authorization;
using TestAuthorizationApp.Models;
using TestAuthorizationApp.Services;

namespace TestAuthorizationApp.Data
{
    public class DefaultUsersInitializer
    {
        private const string _defaultErrorMessage = "Error initializing default users!";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string _defaultUsersEmail;
        private readonly string _defaultUsersPassword;

        public DefaultUsersInitializer(IOptions<Config> config, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _defaultUsersEmail = config.Value.DefaultUsers.Email;
            _defaultUsersPassword = config.Value.DefaultUsers.Password;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public async Task Initialize()
        {
            var administratorId = await EnsureDefaultUser(R.Administrator);
            await EnsureRole(administratorId, R.Administrator);

            var managerId = await EnsureDefaultUser(R.Manager);
            await EnsureRole(managerId, R.Manager);

            var moderatorId = await EnsureDefaultUser(R.Moderator);
            await EnsureRole(moderatorId, R.Moderator);
        }

        private async Task<string> EnsureDefaultUser(string userRole)
        {
            var userName = _defaultUsersEmail.Split('@')[0] + "+" + userRole.ToLower() + "@" +
                       _defaultUsersEmail.Split('@')[1];

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, _defaultUsersPassword);

                if (!result.Succeeded) throw new Exception(_defaultErrorMessage);
            }

            return user.Id;
        }

        private async Task EnsureRole(string id, string role)
        {
            IdentityResult result = null;

            if (!await _roleManager.RoleExistsAsync(role))
            {
                result = await _roleManager.CreateAsync(new IdentityRole(role));

                if (!result.Succeeded) throw new Exception(_defaultErrorMessage);
            }

            var user = await _userManager.FindByIdAsync(id);

            result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded) throw new Exception(_defaultErrorMessage);
        }
    }
}
