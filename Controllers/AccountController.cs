using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Controllers.Base;
using TaskManager.Data.MongoDb;
using TaskManager.Extensions;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.ViewModels.Account;

namespace TaskManager.Controllers
{
    public class AccountController : ControllerWithRedirect
    {
        private readonly MongoDbPlacementRepository placementRepository;
        private readonly MongoDbUserRepository userRepository;

        public AccountController(MongoDbPlacementRepository placementRepository,
                                 MongoDbUserRepository userRepository)
        {
            this.placementRepository = placementRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated is true)
                return RedirectByRole(User.FindFirst("Role").Value);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userRepository.GetByLogin(model.Login);

                if (user is not null && SecurePasswordHasherService.Verify(model.Password, user.HashPassword) is true)
                {
                    await AuthenticateAsync(user);

                    return RedirectByRole(user.Role.ToString());
                }

                ModelState.AddModelError(string.Empty,
                                         "Не удаётся войти. Пожалуйста, проверьте правильность написания логина и пароля.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Placements = await GetPlacementsAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid is true)
            {
                User user = model.CreateUser();

                await userRepository.Add(user);
                await AuthenticateAsync(user);

                return RedirectByRole(user.Role.ToString());
            }

            ViewBag.Placements = await GetPlacementsAsync();

            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

        private async Task<SelectList> GetPlacementsAsync()
            => new(await placementRepository.GetAll(), "Name", "Name");

        private async Task AuthenticateAsync(User user)
        {
            var claims = GetClaims(user);

            ClaimsIdentity id = new(claims,
                                    "ApplicationCookie",
                                    ClaimsIdentity.DefaultNameClaimType,
                                    ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          new ClaimsPrincipal(id));
        }

        private static List<Claim> GetClaims(User user)
            => new()
            {
                new Claim("id", user.Id),
                new Claim("Name", user.Name),
                new Claim("Role", user.Role.ToString())
            };
    }
}