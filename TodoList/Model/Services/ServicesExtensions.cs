using Microsoft.Extensions.DependencyInjection;
using TodoList.Core.Security;

namespace TodoList.Model.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddTodoListServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IUserResolverService), typeof(UserResolverService));

            return serviceCollection;
        }
    }
}