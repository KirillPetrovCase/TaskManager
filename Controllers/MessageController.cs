using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.Controllers.Base;
using TaskManager.Data.MongoDb;
using TaskManager.Extensions;
using TaskManager.Models;
using TaskManager.ViewModels.Message;

namespace TaskManager.Controllers
{
    public class MessageController : ControllerWithRedirect
    {
        private readonly MongoDbChatRepository chatRepository;
        private readonly MongoDbOrderRepository orderRepository;

        public MessageController(MongoDbChatRepository chatRepository,
                              MongoDbOrderRepository orderRepository)
        {
            this.chatRepository = chatRepository;
            this.orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ActiveChat(string orderId)
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
                await orderRepository.UnmarkNewMessageForAdmin(orderId);
            }

            if (User.FindFirstValue("Role") == "User")
            {
                await orderRepository.UnmarkNewMessageForUser(orderId);
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

            if (User.FindFirstValue("Role") == "User")
            {
                await orderRepository.MarkNewMessageForAdmin(model.OrderId);
            }

            return RedirectToAction("ActiveChat", "Message", new { orderId = model.OrderId });
        }

        public async Task<IActionResult> ArchivedChat(string chatId)
        {
            var chat = await chatRepository.GetById(chatId);

            if (chat is null)
            {
                return RedirectByRole(User.FindFirstValue("Role"));
            }

            return View(chat.Messages);
        }

        private static Message CreateMessage(string messageText,
                                      string senderId)
            => new()
            {
                MessageText = messageText,
                SenderId = senderId,
                Time = DateTime.Now
            };

        private static Chat CreateEmptyChat(string orderId)
            => new()
            {
                OrderId = orderId,
                Messages = new List<Message>()
            };

        public IActionResult RedirectToIndex()
                    => RedirectByRole(User.FindFirstValue("Role"));
    }
}