using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Core.Cache;

namespace TodoList.Core.Security
{
    public static class TokenIdentityExtensions
    {
        public static IServiceCollection AddCoreIdentityToken(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<TokenHelper>(s => {
                var contextAcessor = s.GetService<IHttpContextAccessor>();
                var httpContext = contextAcessor.HttpContext;
                var claimsPrincipal = httpContext != null ? httpContext.User : null;
                return new TokenHelper(claimsPrincipal);
            });

            serviceCollection.AddScoped<TokenAuthorizationFilter>();

            serviceCollection.AddMvc(opts => 
            {
                opts.Filters.AddService(typeof(TokenAuthorizationFilter));
            });

            return serviceCollection;
        }
    }
}