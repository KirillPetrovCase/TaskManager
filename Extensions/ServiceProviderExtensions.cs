using Microsoft.Extensions.DependencyInjection;
using TaskManager.Services;

namespace TaskManager.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddDbManagers(this IServiceCollection services)
        {
            services.AddTransient<OrderManager>();
            services.AddTransient<UserManager>();
            services.AddTransient<PlacementManager>();
        }
    }
}