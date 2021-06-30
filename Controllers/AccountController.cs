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
    public class AccountController : Controller
    {
        private readonly PlacementManager placementsDb;
        private readonly UserManager userDb;

        public AccountController(UserManager userDb, PlacementManager placementDb)
        {
            this.userDb = userDb;
            this.placementsDb = placementDb;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userDb.GetUserByUserNameAsync(model.UserName);

                if (user is not null && SecurePasswordHasher.Verify(model.Password, user.HashPassword) is true)
                {
                    await AuthenticateAsync(user);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Не удаётся войти. Пожалуйста, проверьте правильность написания логина и пароля.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Placements = await GetPlacementsAsync(placementsDb);
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

            ViewBag.Placements = await GetPlacementsAsync(placementsDb);
            return View(model);
        }

        private static async Task<SelectList> GetPlacementsAsync(PlacementManager manager) => new SelectList(await manager.GetPlacementAsync(), "Name", "Name");

        private async Task AuthenticateAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("Name", user.UserName),
                new Claim("Role", user.Role.ToString())
            };

            ClaimsIdentity id = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}