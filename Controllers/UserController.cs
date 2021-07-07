using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Data.MongoDb;
using TaskManager.Extensions;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class UserController : ControllerWithRedirect
    {
        private readonly MongoDbOrderRepository orderRepository;
        private readonly MongoDbArchiveRepository archiveRepository;

        public UserController(MongoDbOrderRepository orderRepository,
                              MongoDbArchiveRepository archiveRepository)
        {
            this.orderRepository = orderRepository;
            this.archiveRepository = archiveRepository;
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

            return RedirectByRole(User.FindFirstValue("Role"));
        }

        [HttpGet]
        public IActionResult Privacy() => View();

        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            await orderRepository.Delete(orderId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ConfirmOrder(string orderId)
        {
            var order = await orderRepository.GetById(orderId);
            await archiveRepository.Add(order.CreateArchiveOrder());

            return await DeleteOrder(orderId);
        }

        public async Task<IActionResult> DeclineOrder(string orderId)
        {
            await orderRepository.UnsubscribePerformerFromOrder(orderId);
            return RedirectToAction("Index");
        }
    }
}