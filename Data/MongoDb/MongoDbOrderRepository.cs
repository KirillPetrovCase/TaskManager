using MongoDB.Driver;
using TaskManager.Models;

namespace TaskManager.Data.MongoDb
{
    public class MongoDbOrderRepository : MongoDbRepository<Order, IMongoCollection<Order>>
    {
        public MongoDbOrderRepository() : base(collectionName: "Orders")
        {
        }
    }
}