using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Data.Contracts;
using TaskManager.Models;

namespace TaskManager.Data.MongoDb
{
    public class MongoDbArchiveRepository : MongoDbRepository<ArchiveOrderRecord, IMongoCollection<ArchiveOrderRecord>>
    {
        private readonly IMongoCollection<ArchiveOrderRecord> context;

        public MongoDbArchiveRepository() : base(collectionName: "OrdersArchive")
        {
            context = Сontext;
        }

        public async Task<int> GetTotalDocuments()
            => (int)await context.CountDocumentsAsync(new BsonDocument());

        public async Task<IEnumerable<ArchiveOrderRecord>> GetAllWithSortFilterPagination(string name, int page, int pageSize, SortState sortState)
        {
            var filter = Builders<ArchiveOrderRecord>.Filter.Empty;

            if (string.IsNullOrEmpty(name) is false)
                filter = Builders<ArchiveOrderRecord>.Filter.Where(archOrder => archOrder.OwnerName.Contains(name));

            var sort = sortState switch
            {
                SortState.NameAsc => Builders<ArchiveOrderRecord>.Sort.Ascending(order => order.OwnerName),
                SortState.NameDesc => Builders<ArchiveOrderRecord>.Sort.Descending(order => order.OwnerName),
                SortState.DateAsc => Builders<ArchiveOrderRecord>.Sort.Ascending(order => order.CompleteTime),
                SortState.DateDesc => Builders<ArchiveOrderRecord>.Sort.Descending(order => order.CompleteTime),
                _ => Builders<ArchiveOrderRecord>.Sort.Ascending(order => order.OwnerName)
            };

            return await context.Find(filter).Sort(sort).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();
        }
    }
}