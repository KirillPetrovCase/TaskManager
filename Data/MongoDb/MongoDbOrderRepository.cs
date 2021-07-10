using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Data.Contracts;
using TaskManager.Models;

namespace TaskManager.Data.MongoDb
{
    public class MongoDbOrderRepository : MongoDbRepository<Order, IMongoCollection<Order>>
    {
        private readonly IMongoCollection<Order> context;

        public MongoDbOrderRepository() : base(collectionName: "Orders")
        {
            context = Сontext;
        }

        public async Task<List<Order>> GetAllByOwner(string ownerId)
            => await context.Find(Builders<Order>.Filter.Eq("OwnerId", ownerId)).ToListAsync();

        public async Task SubscribePerformerToOrder(string orderId, string performerId)
        {
            await Update(orderId, "PerformerId", performerId);
            await Update(orderId, "Status", OrderStatus.InWork);
        }

        public async Task UnsubscribePerformerFromOrder(string orderId)
        {
            await Unset(orderId, "PerformerId");
            await Update(orderId, "Status", OrderStatus.Awaiting);
        }

        public async Task MarkOrderAsCompleted(string orderId)
        {
            await Update(orderId, "CompleteTime", DateTime.Now);
            await Update(orderId, "Status", OrderStatus.Completed);
        }

        public async Task MarkNewMessageForAdmin(string orderId)
            => await Update(orderId, "NewMessageForAdmin", true);

        public async Task MarkNewMessageForUser(string orderId)
            => await Update(orderId, "NewMessageForUser", true);

        public async Task UnmarkNewMessageForAdmin(string orderId)
             => await Update(orderId, "NewMessageForAdmin", false);

        public async Task UnmarkNewMessageForUser(string orderId)
            => await Update(orderId, "NewMessageForUser", false);

        public async Task<List<Order>> GetAllInWorkByPerformerId(string performerId)
            => await context.Find(Builders<Order>.Filter.Eq("PerformerId", performerId) & Builders<Order>.Filter.Eq("Status", OrderStatus.InWork)).ToListAsync();

        public async Task<List<Order>> GetAllNotInWork()
            => await context.Find(Builders<Order>.Filter.Eq("Status", OrderStatus.Awaiting)).ToListAsync();

        public async Task<List<Order>> GetAllComletedWorkByPerformerId(string performerId)
            => await context.Find(Builders<Order>.Filter.Eq("PerformerId", performerId) & Builders<Order>.Filter.Eq("Status", OrderStatus.Completed)).ToListAsync();
    }
}