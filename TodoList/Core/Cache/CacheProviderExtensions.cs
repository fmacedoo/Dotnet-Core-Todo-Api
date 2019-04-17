using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Core.Cache.Providers;

namespace TodoList.Core.Cache
{
    public static class CacheProviderExtensions
    {
        public static IServiceCollection AddCoreCacheProviders(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            if (configuration["App:CacheProvider"] == "RedisCacheProvider") {
                serviceCollection.AddScoped(typeof(ICacheProvider), typeof(RedisCacheProvider));
            } else {
                serviceCollection.AddScoped(typeof(ICacheProvider), typeof(FileCacheProvider));
            }

            return serviceCollection;
        }
    }
}