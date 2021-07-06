using MongoDB.Driver;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Data.MongoDb
{
    public class MongoDbChatRepository : MongoDbRepository<Chat, IMongoCollection<Chat>>
    {
        private readonly IMongoCollection<Chat> context;

        public MongoDbChatRepository() : base(collectionName: "Chats")
        {
            context = Сontext;
        }

        public async Task<Chat> GetByOrderId(string orderId)
            => await context.Find(Builders<Chat>.Filter.Where(chat => chat.OrderId == orderId)).FirstOrDefaultAsync();
    }
}