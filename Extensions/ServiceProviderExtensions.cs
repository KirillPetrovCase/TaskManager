using Microsoft.Extensions.DependencyInjection;
using TaskManager.Data.MongoDb;

namespace TaskManager.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddDbManagers(this IServiceCollection services)
        {
            services.AddScoped<MongoDbArchiveRepository>();
            services.AddScoped<MongoDbChatRepository>();
            services.AddScoped<MongoDbOrderRepository>();
            services.AddScoped<MongoDbPlacementRepository>();
            services.AddScoped<MongoDbUserRepository>();
        }
    }
}