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
            var administratorId = await EnsureUser(serviceProvider, Role.Administrator, defaultPassword);
            await EnsureRole(serviceProvider, administratorId, Role.Administrator);

            var managerId = await EnsureUser(serviceProvider, Role.Manager, defaultPassword);
            await EnsureRole(serviceProvider, managerId, Role.Manager);

            var moderatorId = await EnsureUser(serviceProvider, Role.Moderator, defaultPassword);
            await EnsureRole(serviceProvider, moderatorId, Role.Moderator);
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string userName, string userPassword)
        {
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
