using MongoDB.Driver;
using TaskManager.Models;

namespace TaskManager.Data.MongoDb
{
    public class MongoDbPlacementRepository : MongoDbRepository<Placement, IMongoCollection<Placement>>
    {
        public MongoDbPlacementRepository() : base(collectionName: "Placements")
        {
        }
    }
}