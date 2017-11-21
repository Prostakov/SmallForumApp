﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TestAuthorizationApp.Models;

namespace TestAuthorizationApp.Authorization
{
    public class UserAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, ApplicationUser>
    {
        private readonly string _administratorRoleId;
        private readonly string _managerRoleId;
        private readonly string _moderatorRoleId;

        public UserAuthorizationHandler(RoleManager<IdentityRole> roleManager)
        {
            var roles = roleManager.Roles.ToList();
            _administratorRoleId = roles.First(r => r.Name == R.Administrator).Id;
            _managerRoleId = roles.First(r => r.Name == R.Manager).Id;
            _moderatorRoleId = roles.First(r => r.Name == R.Moderator).Id;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, ApplicationUser user)
        {
            if (context.User == null)
            {
                return Task.FromResult(0);
            }

            // Administrators see everything
            if (context.User.IsInRole(R.Administrator))
            {
                context.Succeed(requirement);
            }

            // Managers see only moderators and simple users
            if (context.User.IsInRole(R.Manager))
            {
                if (user.Roles.All(r => r.RoleId != _administratorRoleId && r.RoleId != _managerRoleId))
                {
                    context.Succeed(requirement);
                }
            }

            // Moderators see only simple users
            if (context.User.IsInRole(R.Moderator))
            {
                if (user.Roles.All(r => r.RoleId != _administratorRoleId && r.RoleId != _managerRoleId && r.RoleId != _moderatorRoleId))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.FromResult(0);
        }
    }
}
