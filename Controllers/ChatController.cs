using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManager.Data.MongoDb;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class ChatController : Controller
    {
        private readonly MongoDbChatRepository chatRepository;
        private readonly MongoDbOrderRepository orderRepoitory;

        public ChatController(MongoDbChatRepository chatRepository,
                              MongoDbOrderRepository orderRepoitory)
        {
            this.chatRepository = chatRepository;
            this.orderRepoitory = orderRepoitory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string orderId)
        {
            var chat = await chatRepository.GetByOrderId(orderId);

            if (chat is null)
                chat = CreateEmptyChat(orderId);

            return View(chat);
        }

        private Chat CreateEmptyChat(string orderId)
            => new()
            {
                OrderId = orderId
            };
    }
}