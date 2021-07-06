using System;
using TaskManager.Data.Enums;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.ViewModels;

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
                Role = Roles.User
            };
    }
}