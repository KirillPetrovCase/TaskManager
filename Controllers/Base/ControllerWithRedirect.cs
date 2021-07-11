using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers.Base
{
    public abstract class ControllerWithRedirect : Controller
    {
        public IActionResult RedirectByRole(string roleName)
        {
            if (roleName == "Administrator")
                return RedirectToAction("ActiveOrders", "Admin");

            return RedirectToAction("UserOrders", "User");
        }
    }
}