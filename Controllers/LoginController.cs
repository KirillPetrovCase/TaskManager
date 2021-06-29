using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
            }

            return View(model);
        }
    }
}