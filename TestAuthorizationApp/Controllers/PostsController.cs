using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAuthorizationApp.Data;

namespace TestAuthorizationApp.Controllers
{
    public class PostsController
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
