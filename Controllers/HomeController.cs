using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Data.Enums;
using TaskManager.Data.MongoDb;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly MongoDbOrderRepository orderRepository;

        public HomeController(MongoDbOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await orderRepository.GetAll();
            return View(orders);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            var order = CreateOrderFromModel(model);
            await orderRepository.Add(order);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Privacy() => View();

        private Order CreateOrderFromModel(OrderViewModel model)
        {
            return new()
            {
                Description = model.Description,
                Deadline = model.DeadLine,
                AuthorId = User.FindFirstValue("id"),
                PerformerId = null,
                Message = null,
                Status = OrderStatus.Awaiting,
                RegisterTime = DateTime.Now
            };
        }
    }
}