using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Data.MongoDb
{
    public abstract class MongoDbRepository<TDocument, TContext> : IRepository<TDocument>
        where TDocument : class, IDocument
        where TContext : IMongoCollection<TDocument>
    {
        private readonly TContext context;

        public TContext Сontext => context;

        protected MongoDbRepository(string collectionName, string connectionStringSectionName = "DefaultConnectionString")
        {
            string connectionString = Startup.StaticConfiguration.GetConnectionString(connectionStringSectionName);

            MongoUrlBuilder mongoUrlBuilder = new(connectionString);
            MongoClient mongoClient = new(connectionString);

            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(mongoUrlBuilder.DatabaseName);

            context = (TContext)mongoDatabase.GetCollection<TDocument>(collectionName);
        }

        public async Task<List<TDocument>> GetAll()
            => await context.Find(Builders<TDocument>.Filter.Empty).ToListAsync();

        public async Task<TDocument> GetById(string id)
            => await context.Find(Builders<TDocument>.Filter.Where(e => e.Id == id)).FirstOrDefaultAsync();

        public async Task Add(TDocument entity)
            => await context.InsertOneAsync(entity);

        public async Task Delete(string id)
            => await context.DeleteOneAsync(Builders<TDocument>.Filter.Eq(e => e.Id, id));

        public async Task Update<TFieldValue>(string id, string field, TFieldValue value)
            => await context.UpdateOneAsync(Builders<TDocument>.Filter.Where(e => e.Id == id), Builders<TDocument>.Update.Set(field, value));

        public async Task Unset(string id, string field)
            => await context.UpdateOneAsync(Builders<TDocument>.Filter.Where(e => e.Id == id), Builders<TDocument>.Update.Unset(field));

        public async Task Update(TDocument entity)
        {
            await Delete(entity.Id);
            await Add(entity);
        }
    }
}