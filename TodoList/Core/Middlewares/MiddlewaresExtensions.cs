using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoList.Core.Security;

namespace TodoList.Core.Middlewares
{
    public static class MiddlewaresExtension
    {
        private static readonly string secretKey = "+J7{,7]&e7F-b9nckOgwuBJL9zyFae";
        private static SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

        public static IApplicationBuilder UseCoreMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiExceptionMiddleware>();

            var options = new TokenProviderOptions {
                Path = "/api/token",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                TokenResolverServiceFactory = () => {
                    var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
                    return serviceScope.ServiceProvider.GetRequiredService<ITokenResolverService>();
                }
            };

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));
            
            return app;
        }
    }
}