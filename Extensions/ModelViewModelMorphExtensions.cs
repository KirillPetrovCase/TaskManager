using System;
using TaskManager.Data.Contracts;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.ViewModels.Account;
using TaskManager.ViewModels.Message;
using TaskManager.ViewModels.User;

namespace TaskManager.Extensions
{
    public static class ModelViewModelMorphExtensions
    {
        public static Order CreateOrder(this CreateOrderViewModel model, string ownerId)
            => new()
            {
                Description = model.Description,
                OwnerId = ownerId,
                RegisterTime = DateTime.Now,
                Deadline = model.Deadline,
                Status = OrderStatus.Awaiting,
                NewMessageForAdmin = false,
                NewMessageForUser = false
            };

        public static User CreateUser(this RegisterViewModel model)
            => new()
            {
                Login = model.Login,
                Name = model.Name,
                HashPassword = SecurePasswordHasherService.Hash(model.Password),
                Placement = model.Placement,
                Role = Role.User
            };

        public static ChatViewModel CreateChatViewModel(this Chat chat)
            => new()
            {
                Chat = chat,
                SendMessageViewModel = new()
                {
                    ChatId = chat.Id,
                    OrderId = chat.OrderId
                }
            };


        public static ArchiveOrderRecord CreateArchiveOrder(this Order order, string ownerName)
        => new()
        {
            ArchivedTime = DateTime.Now,
            ChatId = order.ChatId,
            CompleteTime = order.CompleteTime,
            Deadline = order.Deadline,
            Description = order.Description,
            OwnerName = ownerName,
            OwnerId = order.OwnerId,
            PerformerId = order.PerformerId,
            RegisterTime = order.RegisterTime
        };
    }
}