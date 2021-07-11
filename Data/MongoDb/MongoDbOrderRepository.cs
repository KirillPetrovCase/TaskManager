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

        public async Task<IEnumerable<Order>> GetAllByOwnerAsync(string id)
            => await context.Find(Builders<Order>.Filter.Where(order => order.OwnerId == id)).ToListAsync();

        public async Task<IEnumerable<Order>> GetAllAwaitingsAsync()
            => await context.Find(Builders<Order>.Filter.Where(order => order.Status == OrderStatus.Awaiting)).ToListAsync();

        public async Task<int> CountAwaitingsAsync()
            => (int)await context.CountDocumentsAsync(Builders<Order>.Filter.Where(order => order.Status == OrderStatus.Awaiting));

        public async Task<int> CountInWorkByIdAsync(string id)
            => (int)await context.CountDocumentsAsync(Builders<Order>.Filter.Where(order => order.PerformerId == id && order.Status == OrderStatus.InWork));
        

        public async Task<IEnumerable<Order>> GetAllInWorkByPerformerAsync(string id)
            => await context.Find(Builders<Order>.Filter.Where(order => order.PerformerId == id && order.Status == OrderStatus.InWork)).ToListAsync();

        public async Task SubscribePerformerToOrderAsync(string orderId, string performerId, string performerName)
        {
            await UpdateAsync(orderId, "PerformerId", performerId);
            await UpdateAsync(orderId, "PerformerName", performerName);
            await UpdateAsync(orderId, "Status", OrderStatus.InWork);
        }

        public async Task UnsubscribePerformerFromOrderAsync(string orderId)
        {
            await UnsetAsync(orderId, "PerformerId");
            await UnsetAsync(orderId, "PerformerName");
            await UpdateAsync(orderId, "Status", OrderStatus.Awaiting);
        }

        public async Task MarkOrderAsCompletedAsync(string orderId)
        {
            await UpdateAsync(orderId, "CompleteTime", DateTime.Now);
            await UpdateAsync(orderId, "Status", OrderStatus.Completed);
        }

        public async Task MarkNewMessageForAdminAsync(string orderId)
            => await UpdateAsync(orderId, "NewMessageForAdmin", true);

        public async Task MarkNewMessageForUserAsync(string orderId)
            => await UpdateAsync(orderId, "NewMessageForUser", true);

        public async Task UnmarkNewMessageForAdminAsync(string orderId)
             => await UpdateAsync(orderId, "NewMessageForAdmin", false);

        public async Task UnmarkNewMessageForUserAsync(string orderId)
            => await UpdateAsync(orderId, "NewMessageForUser", false);
    }
}