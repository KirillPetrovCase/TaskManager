using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Extensions;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class OrderManager
    {
        private readonly IMongoCollection<Order> OrdersDb;

        public OrderManager()
        {
            IMongoDatabase mongoDatabase = DbExtensions.GetDatabase();

            OrdersDb = mongoDatabase.GetCollection<Order>("Orders");
        }

        public async Task<List<Order>> GetOrdersAsync(int pageNumber = 0, int pageSize = 10)
        {
            var filter = Builders<Order>.Filter.Empty;

            return await OrdersDb.Find(filter)
                                 .Skip(pageNumber * pageSize)
                                 .Limit(pageSize)
                                 .ToListAsync();
        }
    }
}