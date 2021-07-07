using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers
{
    public abstract class ControllerWithRedirect : Controller
    {
        public IActionResult RedirectByRole(string roleName)
        {
            if (roleName == "Administrator")
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "User");
        }
    }
}