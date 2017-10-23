using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TestAuthorizationApp.Authorization;
using TestAuthorizationApp.Models;

namespace TestAuthorizationApp.Data
{
    public class DefaultUsersInitializer
    {
        private static readonly string ErrorMessage = "Error initializing default users!";
        
        public static async Task Initialize(IServiceProvider serviceProvider, string defaultPassword)
        {
            var administratorId = await EnsureUser(serviceProvider, A.Administrator, defaultPassword);
            await EnsureRole(serviceProvider, administratorId, A.Administrator);

            var managerId = await EnsureUser(serviceProvider, A.Manager, defaultPassword);
            await EnsureRole(serviceProvider, managerId, A.Manager);

            var moderatorId = await EnsureUser(serviceProvider, A.Moderator, defaultPassword);
            await EnsureRole(serviceProvider, moderatorId, A.Moderator);
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string userName, string userPassword)
        {
            userName = "prostakov+" + userName + "@zoho.com";

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = userName };
                var result = await userManager.CreateAsync(user, userPassword);

                if (!result.Succeeded) throw new Exception(ErrorMessage);
            }

            return user.Id;
        }

        private static async Task EnsureRole(IServiceProvider serviceProvider, string id, string role)
        {
            IdentityResult result = null;

            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role))
            {
                result = await roleManager.CreateAsync(new IdentityRole(role));

                if (!result.Succeeded) throw new Exception(ErrorMessage);
            }

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(id);

            result = await userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded) throw new Exception(ErrorMessage);
        }
    }
}
