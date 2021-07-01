using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Privacy() => View();
    }
}