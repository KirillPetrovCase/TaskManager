using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Data.Enums;
using TaskManager.Data.MongoDb;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly MongoDbOrderRepository orderRepository;
        private readonly MongoDbUserRepository userRepository;

        public AdminController(MongoDbOrderRepository orderRepository, MongoDbUserRepository userRepository)
        {
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
        }

        [Authorize("AdministratorRoleAccess")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await orderRepository.GetAll();
            var activeOrders = GetActiveOrdersViewModel(orders);

            return View(activeOrders);
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
                    yourOrders.Add(CreateModelFromOrder(order, "_perfomingOrderOptions").Result);
                }
                else
                {
                    otherOrders.Add(CreateModelFromOrder(order, "_awaitingOrderOptions").Result);
                }
            }

            ActiveOrdersViewModel model = new()
            {
                YourActiveOrders = yourOrders,
                OtherActiveOrders = otherOrders
            };

            return model;
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

        public async Task<IActionResult> SubscribeToOrder(string id)
        {
            await orderRepository.Update(id, "PerformerId", User.FindFirstValue("id"));
            await orderRepository.Update(id, "Status", (int)OrderStatus.Performing);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UnsubscribeFromOrder(string id)
        {
            await orderRepository.Unset(id, "PerformerId");
            await orderRepository.Update(id, "Status", (int)OrderStatus.Awaiting);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CancelOrder(string id)
        {
            await orderRepository.Update(id, "Status", (int)OrderStatus.Cancelled);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CompleteOrder(string id)
        {
            await orderRepository.Update(id, "Status", (int)OrderStatus.Completed);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> MessagingToAuthor(string id, string message)
        {
            await orderRepository.Update(id, "Message", message);
            return RedirectToAction("Index");
        }
    }
}