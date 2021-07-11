using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Controllers.Base;
using TaskManager.Data.MongoDb;
using TaskManager.Extensions;
using TaskManager.ViewModels.User;

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
        public async Task<IActionResult> UserOrders()
            => View(await orderRepository.GetAllByOwnerAsync(User.FindFirstValue("id")));

        [HttpGet]
        public IActionResult CreateOrder() => View(new CreateOrderViewModel() { Deadline = System.DateTime.Now.Date });

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model)
        {
            if (ModelState.IsValid is true)
            {
                var id = User.FindFirstValue("id");
                var name = User.FindFirstValue("Name");

                await orderRepository.AddAsync(model.CreateOrder(id, name));

                return RedirectToAction("UserOrders");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Privacy() => View();

        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            await orderRepository.DeleteAsync(orderId);

            return RedirectToAction("UserOrders");
        }

        public async Task<IActionResult> ConfirmOrder(string orderId)
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            await archiveRepository.AddAsync(order.CreateArchiveOrder());

            return await DeleteOrder(orderId);
        }

        public async Task<IActionResult> DeclineOrder(string orderId)
        {
            await orderRepository.UnsubscribePerformerFromOrderAsync(orderId);

            return RedirectToAction("UserOrders");
        }
    }
}