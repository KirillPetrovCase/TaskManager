using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Data.MongoDb;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly MongoDbOrderRepository orderRepository;

        public AdminController(MongoDbOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [Authorize("AdministratorRoleAccess")]
        [HttpGet]
        public async Task<IActionResult> Index()
            => View(new AdminOrderViewModel()
            {
                CompletedOrders = await orderRepository.GetAllComletedWorkByPerformerId(User.FindFirstValue("id")),
                OrdersInWork = await orderRepository.GetAllInWorkByPerformerId(User.FindFirstValue("id")),
                OrdersNotInWork = await orderRepository.GetAllNotInWork()
            });

        [Authorize("AdministratorRoleAccess")]
        [HttpGet]
        public async Task<IActionResult> Archive() => View();

        public async Task<IActionResult> SubscribeToOrder(string id)
        {
            await orderRepository.SubscribePerformerToOrder(id, User.FindFirstValue("id"));
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> UnsubscribeFromOrder(string id)
        {
            await orderRepository.UnsubscribePerformerFromOrder(id);
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> CompleteOrder(string id)
        {
            await orderRepository.MarkOrderAsCompleted(id);
            return RedirectToAction("Index", "Admin");
        }
    }
}