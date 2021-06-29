using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Extensions;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager userDb;
        private readonly PlacementManager placementsDb;

        public RegisterController(UserManager userDb, PlacementManager placementDb)
        {
            this.userDb = userDb;
            this.placementsDb = placementDb;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Placements = await GetSelectListAsync(placementsDb);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid is true)
            {
                User user = new()
                {
                    Name = model.Name,
                    UserName = model.UserName,
                    HashPassword = SecurePasswordHasher.Hash(model.Password),
                    Role = Roles.User,
                    Post = model.Post,
                    Placement = model.Placement,
                    Orders = null
                };

                await userDb.CreateAsync(user);
                await AuthenticateAsync(user);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Placements = await GetSelectListAsync(placementsDb);
            return View(model);
        }

        private static async Task<SelectList> GetSelectListAsync(PlacementManager manager) => new SelectList(await manager.GetPlacementAsync(), "Name", "Name");

        public async Task AuthenticateAsync(User user)
        {
            var claims = new List<Claim> { new Claim("Role", user.Role.ToString()) };

            ClaimsIdentity id = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}