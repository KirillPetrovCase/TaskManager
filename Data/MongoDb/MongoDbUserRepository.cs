using MongoDB.Driver;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Data.MongoDb
{
    public class MongoDbUserRepository : MongoDbRepository<User, IMongoCollection<User>>
    {
        private readonly IMongoCollection<User> context;

        public MongoDbUserRepository() : base(collectionName: "Users")
        {
            context = Сontext;
        }

        public async Task<User> GetByLogin(string userLogin)
            => await context.Find(Builders<User>.Filter.Where(user => user.Login == userLogin)).FirstOrDefaultAsync();

        public string GetNameById(string id)
            => GetById(id).Result.Name;
    }
}