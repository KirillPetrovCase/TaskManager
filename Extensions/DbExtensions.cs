using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace TaskManager.Extensions
{
    public static class DbExtensions
    {
        public static IMongoDatabase GetDatabase()
        {
            string connectionString = Startup.StaticConfiguration.GetConnectionString("DefaultConnectionString");

            MongoUrlBuilder mongoUrlBuilder = new(connectionString);
            MongoClient mongoClient = new(connectionString);

            return mongoClient.GetDatabase(mongoUrlBuilder.DatabaseName);
        }
    }
}