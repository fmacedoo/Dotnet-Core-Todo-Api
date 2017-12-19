using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TodoList.Core.Data
{
    public static class DataExtensions
    {
        public static IServiceCollection AddCoreIdentity<TContext, TContextInitializer>(this IServiceCollection serviceCollection, IConfiguration configuration)
            where TContext : CoreIdentityDbContext
            where TContextInitializer : class, IInitializer
        {
            serviceCollection.AddEntityFrameworkSqlite().AddDbContext<TContext>(options => {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });

            serviceCollection.AddIdentity<CoreIdentityUser, CoreRole>(o => {
                    // configure identity options
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<TContext>()
                .AddDefaultTokenProviders();

            serviceCollection.AddScoped<TContextInitializer>();

            return serviceCollection;
        }

        public static IServiceCollection AddCoreServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection;
        }

        public static IServiceCollection AddCoreRepositories(this IServiceCollection serviceCollection)
        {
            //serviceCollection.AddScoped(typeof(IRepository<>), typeof(AnagoEFRepository<>));

            return serviceCollection;
        }

        public static IApplicationBuilder UseCoreInitilizer<TContextInitializer>(this IApplicationBuilder app)
            where TContextInitializer : class, IInitializer
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var initializer = serviceScope.ServiceProvider.GetService<TContextInitializer>();       
                var task = initializer.SeedAsync();
                task.Wait();
            }

            return app;
        }
    }
}