using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TestAuthorizationApp.Authorization;

namespace TestAuthorizationApp.Controllers
{
    public class TestController : Controller
    {
        [Authorize(Policy = A.Administrator)]
        public IActionResult IndexForAdministrators()
        {
            return View();
        }

        [Authorize(Policy = A.Manager)]
        public IActionResult IndexForManagers()
        {
            return View();
        }

        [Authorize(Policy = A.Moderator)]
        public IActionResult IndexForModerators()
        {
            return View();
        }

        [Authorize(Policy = A.User)]
        public IActionResult IndexForUsers()
        {
            return View();
        }
        
        public IActionResult IndexForNotAuthenticatedUsers()
        {
            return View();
        }
    }
}