using MongoDB.Driver;
using TaskManager.Models;

namespace TaskManager.Data.MongoDb
{
    public class MongoDbArchiveRepository : MongoDbRepository<ArchiveOrderRecord, IMongoCollection<ArchiveOrderRecord>>
    {
        public MongoDbArchiveRepository() : base(collectionName: "Orders")
        {
        }
    }
}