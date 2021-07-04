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
            var orders = await orderRepository.GetAll();
            var activeOrders = GetActiveOrdersViewModel(orders);

            return View(activeOrders);
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

        public async Task<IActionResult> SubscribeToOrder(string id)
        {
            await orderRepository.Update<string>(id, "PerformerId", User.FindFirstValue("id"));
            await orderRepository.Update<int>(id, "Status", ((int)OrderStatus.Performing));
            return RedirectToAction("Index");
        }

        private ActiveOrdersViewModel GetActiveOrdersViewModel(List<Order> orders)
        {
            string userId = User.FindFirstValue("id");

            List<ShowOrderViewModel> yourOrders = new();
            List<ShowOrderViewModel> otherOrders = new();

            foreach (var order in orders)
            {
                if (order.PerformerId == userId)
                {
                    yourOrders.Add(CreateModelFromOrder(order).Result);
                }
                else
                {
                    otherOrders.Add(CreateModelFromOrder(order).Result);
                }
            }

            ActiveOrdersViewModel model = new()
            {
                YourActiveOrders = yourOrders,
                OtherActiveOrders = otherOrders
            };

            return model;
        }

        private async Task<ShowOrderViewModel> CreateModelFromOrder(Order order)
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
                PerformerId = order.PerformerId
            };
        }
    }
}