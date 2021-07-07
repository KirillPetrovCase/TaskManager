using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Data.MongoDb;
using TaskManager.Extensions;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class ChatController : ControllerWithRedirect
    {
        private readonly MongoDbChatRepository chatRepository;
        private readonly MongoDbOrderRepository orderRepository;

        public ChatController(MongoDbChatRepository chatRepository,
                              MongoDbOrderRepository orderRepository)
        {
            this.chatRepository = chatRepository;
            this.orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string orderId)
        {
            var chat = await chatRepository.GetByOrderId(orderId);

            if (chat is null)
            {
                if (orderId is not null)
                {
                    chat = CreateEmptyChat(orderId);

                    await chatRepository.Add(chat);
                    await orderRepository.Update(orderId, "ChatId", chatRepository.GetByOrderId(orderId).Result.Id);
                }
                else
                {
                    return RedirectByRole(User.FindFirstValue("Role"));
                }
            }

            if (User.FindFirstValue("Role") == "Administrator")
            {
                await orderRepository.UnmarkNewMessageForUser(orderId);
            }
            else
            {
                await orderRepository.UnmarkNewMessageForAdmin(orderId);
            }

            return View(chat.CreateChatViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageViewModel model)
        {
            var message = CreateMessage(model.MessageText, User.FindFirstValue("id"));
            await chatRepository.AddMessageToChat(model.ChatId, message);
            if (User.FindFirstValue("Role") == "Administrator")
            {
                await orderRepository.MarkNewMessageForUser(model.OrderId);
            }
            else
            {
                await orderRepository.MarkNewMessageForAdmin(model.OrderId);
            }

            return RedirectToAction("Index", "Chat", new { orderId = model.OrderId });
        }

        private Message CreateMessage(string messageText,
                                      string senderId)
            => new()
            {
                MessageText = messageText,
                SenderId = senderId,
                Time = DateTime.Now
            };

        private Chat CreateEmptyChat(string orderId)
            => new()
            {
                OrderId = orderId,
                Messages = new List<Message>()
            };

        public IActionResult RedirectToIndex()
                    => RedirectByRole(User.FindFirstValue("Role"));
    }
}