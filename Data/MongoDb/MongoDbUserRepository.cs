using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Data.Contracts;
using TaskManager.Models;

namespace TaskManager.Data.MongoDb
{
    public class MongoDbUserRepository : MongoDbRepository<Owner, IMongoCollection<Owner>>
    {
        private readonly IMongoCollection<Owner> context;

        public MongoDbUserRepository() : base(collectionName: "Users")
        {
            context = Сontext;
        }

        public async Task<Owner> GetByLoginAsync(string userLogin)
            => await context.Find(Builders<Owner>.Filter.Where(user => user.Login == userLogin)).FirstOrDefaultAsync();

        public async Task<IEnumerable<Owner>> GetAllUsersAsync()
            => await context.Find(Builders<Owner>.Filter.Where(user => user.Login != "admin")).ToListAsync();

        public async Task ChangeRoleToAdmin(string id)
            => await UpdateAsync(id, "Role", Role.Administrator);

        public async Task ChangeRoleToUser(string id)
            => await UpdateAsync(id, "Role", Role.User);

        public async Task<IEnumerable<Owner>> GetAllWithSortFilterPaginationAsync(string name, int page, int pageSize, SortState sortState)
        {
            var filter = Builders<Owner>.Filter.Where(user => user.Login != "admin");

            if (string.IsNullOrEmpty(name) is false)
                filter &= Builders<Owner>.Filter.Where(user => user.Name.Contains(name));

            var sort = sortState switch
            {
                SortState.NameAsc => Builders<Owner>.Sort.Ascending(user => user.Name),
                SortState.NameDesc => Builders<Owner>.Sort.Descending(user => user.Name),
                _ => Builders<Owner>.Sort.Ascending(user => user.Name)
            };

            return await context.Find(filter).Sort(sort).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync();
        }
    }
}