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
        private readonly MongoDbUserRepository userRepository;

        public AdminController(MongoDbOrderRepository orderRepository,
                               MongoDbArchiveRepository archiveRepository,
                               MongoDbUserRepository userRepository)
        {
            this.orderRepository = orderRepository;
            this.archiveRepository = archiveRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ActiveOrders()
            => View(await orderRepository.GetAllInWorkByPerformerAsync(User.FindFirstValue("id")));

        [HttpGet]
        public async Task<IActionResult> AwaitingOrders()
            => View(await orderRepository.GetAllAwaitingsAsync());

        [HttpGet]
        public async Task<IActionResult> OrderArchive(string name, string performer, int page = 1, int pageSize = 5, SortState sortState = SortState.NameAsc)
        {
            var totalDocuments = await archiveRepository.GetCountDocumentsAsync();
            var records = await archiveRepository.GetAllWithSortFilterPaginationAsync(name, performer, page, pageSize, sortState);

            return View(new ArchiveViewModel()
            {
                PageViewModel = new PageViewModel(page, totalDocuments, pageSize),
                SortViewModel = new SortViewModel(sortState),
                FilterViewModel = new FilterViewModel(name, performer),
                Records = records
            });
        }

        [HttpGet]
        public async Task<IActionResult> UserList(string name, int page = 1, int pageSize = 5, SortState sortState = SortState.NameAsc)
        {
            var totalDocuments = await userRepository.GetCountDocumentsAsync();
            var users = await userRepository.GetAllWithSortFilterPaginationAsync(name, page, pageSize, sortState);

            return View(new UserListViewModel()
            {
                PageViewModel = new PageViewModel(page, totalDocuments, pageSize),
                SortViewModel = new SortViewModel(sortState),
                FilterViewModel = new FilterViewModel(name),
                Users = users
            });
        }

        public async Task<IActionResult> SubscribeToOrder(string orderId)
        {
            await orderRepository.SubscribePerformerToOrderAsync(orderId, User.FindFirstValue("id"), User.FindFirstValue("Name"));
            return RedirectToAction("ActiveOrders", "Admin");
        }

        public async Task<IActionResult> UnsubscribeFromOrder(string orderId)
        {
            await orderRepository.UnsubscribePerformerFromOrderAsync(orderId);
            return RedirectToAction("AwaitingOrders", "Admin");
        }

        public async Task<IActionResult> CompleteOrder(string orderId)
        {
            await orderRepository.MarkOrderAsCompletedAsync(orderId);
            return RedirectToAction("ActiveOrders", "Admin");
        }

        public async Task<IActionResult> ChangeRoleToAdmin(string id)
        {
            await userRepository.ChangeRoleToAdmin(id);
            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> ChangeRoleToUser(string id)
        {
            await userRepository.ChangeRoleToUser(id);
            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            await userRepository.DeleteAsync(id);
            return RedirectToAction("UserList");
        }
    }
}