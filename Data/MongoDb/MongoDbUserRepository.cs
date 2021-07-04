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

        public async Task<User> GetByUserName(string userName)
            => await context.Find(Builders<User>.Filter.Where(e => e.UserName == userName)).FirstOrDefaultAsync();

        public async Task<string> GetNamebyId(string id)
        {
            User user = await Get(id);
            return user.Name;
        }
    }
}