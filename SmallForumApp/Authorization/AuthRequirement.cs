using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace SmallForumApp.Authorization
{
    public class AuthRequirement
    {
        public static OperationAuthorizationRequirement UserRead => new OperationAuthorizationRequirement {Name = "UserRead"};
    }
}
