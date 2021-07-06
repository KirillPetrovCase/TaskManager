using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Data.MongoDb;
using TaskManager.Extensions;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class UserController : Controller
    {
        private readonly MongoDbOrderRepository orderRepository;

        public UserController(MongoDbOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await orderRepository.GetAllByOwner(User.FindFirstValue("id")));

        [HttpGet]
        public IActionResult CreateOrder() => View();

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model)
        {
            var id = User.FindFirstValue("id");
            await orderRepository.Add(model.CreateOrder(id));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Privacy() => View();

        public async Task<IActionResult> DeleteOrder(string id)
        {
            await orderRepository.Delete(id);
            return RedirectToAction("Index", "User");
        }
    }
}