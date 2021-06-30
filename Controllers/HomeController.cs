using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly OrderManager orderManager;

        public HomeController(OrderManager orderManager)
        {
            this.orderManager = orderManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Order> orders = await orderManager.GetOrdersAsync();
            return View(orders);
        }

        [HttpGet]
        public IActionResult Privacy() => View();

        [HttpGet]
        public IActionResult CreateOrder() => View();

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(OrderCreateViewModel model)
        {

            return RedirectToAction("Index", "Home");
        }
    }
}