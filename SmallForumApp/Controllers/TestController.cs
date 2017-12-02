using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmallForumApp.Authorization;

namespace SmallForumApp.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = R.Administrator)]
        public IActionResult IndexForAdministrators()
        {
            return View();
        }

        [Authorize(Policy = R.Manager)]
        public IActionResult IndexForManagers()
        {
            return View();
        }

        [Authorize(Policy = R.Moderator)]
        public IActionResult IndexForModerators()
        {
            return View();
        }

        [Authorize]
        public IActionResult IndexForAuthenticatedUsers()
        {
            return View();
        }
    }
}