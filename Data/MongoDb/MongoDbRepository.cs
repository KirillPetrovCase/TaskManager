using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Data.MongoDb
{
    public abstract class MongoDbRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : IMongoCollection<TEntity>
    {
        private readonly TContext context;

        public TContext Сontext => context;

        protected MongoDbRepository(string collectionName, string connectionStringSectionName = "DefaultConnectionString")
        {
            string connectionString = Startup.StaticConfiguration.GetConnectionString(connectionStringSectionName);

            MongoUrlBuilder mongoUrlBuilder = new(connectionString);
            MongoClient mongoClient = new(connectionString);

            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(mongoUrlBuilder.DatabaseName);

            context = (TContext)mongoDatabase.GetCollection<TEntity>(collectionName);
        }

        public async Task<List<TEntity>> GetAll()
            => await context.Find(Builders<TEntity>.Filter.Empty).ToListAsync();

        public async Task<TEntity> Get(string id)
            => await context.Find(Builders<TEntity>.Filter.Where(e => e.Id == id)).FirstOrDefaultAsync();

        public async Task Add(TEntity entity)
            => await context.InsertOneAsync(entity);

        public async Task Delete(string id)
            => await context.DeleteOneAsync(Builders<TEntity>.Filter.Eq(e => e.Id, id));

        public async Task Update(TEntity entity)
        {
            await Delete(entity.Id);
            await Add(entity);
        }
    }
}