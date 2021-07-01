using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Data.MongoDb
{
    public class MongoDbOrderRepository : MongoDbRepository<Order, IMongoCollection<Order>>
    {
        private readonly IMongoCollection<Order> context;

        public MongoDbOrderRepository() : base(collectionName: "Orders")
        {
            context = Сontext;
        }
    }
}