using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using TodoList.Model.Data;
using Microsoft.AspNetCore.Identity;
using TodoList.Core.Security;
using Microsoft.AspNetCore.Http;
using TodoList.Core.Cache;
using TodoList.Core.Data;
using TodoList.Core.Middlewares;
using TodoList.Model.Services;
using ServiceOrders.Core.Filters;

namespace TodoList
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider => Configuration);

            services.AddCoreCacheProviders(Configuration);

            services.AddCoreIdentity<TodoListContext, TodoListInitializer>(Configuration);

            services.AddCoreIdentityToken();

            services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials() );
                });

            services.AddCoreRepositories();

            services.AddCoreServices();

            services.AddTodoListServices();

            services.AddScoped<TokenAuthorizationFilter>();
            services.AddScoped<ApiExceptionFilter>();

            // Add framework services.
            services.AddMvc(opts =>
            {
                //Here it is being added globally. 
                //Could be used as attribute on selected controllers instead
                opts.Filters.AddService(typeof(TokenAuthorizationFilter));
                opts.Filters.AddService(typeof(ApiExceptionFilter));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCoreMiddlewares();

            app.UseAuthentication();
            
            app.UseMvc();

            app.UseCoreInitilizer<TodoListInitializer>();
        }
    }
}
