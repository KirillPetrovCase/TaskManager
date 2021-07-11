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

        public long GetCountDocuments()
            => context.CountDocuments(new BsonDocument());

        public async Task<int> GetCountDocumentsAsync()
            => (int)await context.CountDocumentsAsync(new BsonDocument());

        public async Task<List<TDocument>> GetAllAsync()
            => await context.Find(Builders<TDocument>.Filter.Empty).ToListAsync();

        public async Task<TDocument> GetByIdAsync(string id)
            => await context.Find(Builders<TDocument>.Filter.Where(e => e.Id == id)).FirstOrDefaultAsync();

        public async Task AddAsync(TDocument document)
            => await context.InsertOneAsync(document);

        public void Add(TDocument document)
            => context.InsertOne(document);

        public void AddMany(IEnumerable<TDocument> documents)
            => context.InsertMany(documents);

        public async Task DeleteAsync(string id)
            => await context.DeleteOneAsync(Builders<TDocument>.Filter.Where(d => d.Id == id));

        public async Task UpdateAsync<TFieldValue>(string id, string field, TFieldValue value)
            => await context.UpdateOneAsync(Builders<TDocument>.Filter.Where(d => d.Id == id), Builders<TDocument>.Update.Set(field, value));

        public async Task UnsetAsync(string id, string field)
            => await context.UpdateOneAsync(Builders<TDocument>.Filter.Where(d => d.Id == id), Builders<TDocument>.Update.Unset(field));

        public async Task UpdateAsync(TDocument document)
        {
            await DeleteAsync(document.Id);
            await AddAsync(document);
        }
    }
}