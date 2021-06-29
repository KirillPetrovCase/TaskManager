using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using TaskManager.Extensions;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class OrderManager
    {
        private readonly IMongoCollection<Order> OrdersDb;

        public OrderManager(IConfiguration configuration)
        {
            IMongoDatabase mongoDatabase = DbExtensions.GetDatabase();

            OrdersDb = mongoDatabase.GetCollection<Order>("Orders");
        }
    }
}