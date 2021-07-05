using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        private readonly MongoDbUserRepository userRepository;

        public HomeController(MongoDbOrderRepository orderRepository, MongoDbUserRepository userRepository)
        {
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await orderRepository.GetAllWithFilter("_id", User.FindFirstValue("id"));

            List<ShowOrderViewModel> models = new();

            foreach (var order in orders)
                models.Add(CreateModelFromOrder(order, "_userOrderOptions").Result);

            return View(models);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreatingOrderViewModel model)
        {
            var order = CreateOrderFromModel(model);
            await orderRepository.Add(order);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Privacy() => View();

        private Order CreateOrderFromModel(CreatingOrderViewModel model)
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

        private async Task<ShowOrderViewModel> CreateModelFromOrder(Order order, string optionsViewName)
        {
            return new()
            {
                AuthorName = await userRepository.GetNamebyId(order.AuthorId),
                OrderId = order.Id,
                Description = order.Description,
                Status = order.Status,
                RegisterDate = order.RegisterTime,
                DeadLine = order.Deadline,
                Message = order.Message,
                AuthorId = order.AuthorId,
                PerformerId = order.PerformerId,
                OptionsViewName = optionsViewName
            };
        }
    }
}