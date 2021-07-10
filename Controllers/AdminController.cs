using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Data.Contracts;
using TaskManager.Data.MongoDb;
using TaskManager.ViewModels.Admin;

namespace TaskManager.Controllers
{
    [Authorize("AdministratorRoleAccess")]
    public class AdminController : Controller
    {
        private readonly MongoDbOrderRepository orderRepository;
        private readonly MongoDbArchiveRepository archiveRepository;

        public AdminController(MongoDbOrderRepository orderRepository,
                               MongoDbArchiveRepository archiveRepository)
        {
            this.orderRepository = orderRepository;
            this.archiveRepository = archiveRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(new AdminOrderViewModel()
            {
                CompletedOrders = await orderRepository.GetAllComletedWorkByPerformerId(User.FindFirstValue("id")),
                OrdersInWork = await orderRepository.GetAllInWorkByPerformerId(User.FindFirstValue("id")),
                OrdersNotInWork = await orderRepository.GetAllNotInWork()
            });

        public async Task<IActionResult> OrderArchive(string name, int page = 1, int pageSize = 5, SortState sortState = SortState.NameAsc)
        {
            var totalDocuments = await archiveRepository.GetTotalDocuments();
            var records = await archiveRepository.GetAllWithSortFilterPagination(name, page, pageSize, sortState);

            return View(new ArchiveViewModel()
            {
                PageViewModel = new PageViewModel(page, totalDocuments, pageSize),
                SortViewModel = new SortViewModel(sortState),
                FilterViewModel = new FilterViewModel(name),
                Records = records
            });
        }

        public async Task<IActionResult> SubscribeToOrder(string orderId)
        {
            await orderRepository.SubscribePerformerToOrder(orderId, User.FindFirstValue("id"));
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> UnsubscribeFromOrder(string orderId)
        {
            await orderRepository.UnsubscribePerformerFromOrder(orderId);
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> CompleteOrder(string orderId)
        {
            await orderRepository.MarkOrderAsCompleted(orderId);
            return RedirectToAction("Index", "Admin");
        }
    }
}