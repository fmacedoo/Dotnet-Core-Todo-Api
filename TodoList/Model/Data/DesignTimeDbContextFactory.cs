using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TodoList.Model.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TodoListContext>
    {
        public TodoListContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
    
            var builder = new DbContextOptionsBuilder<TodoListContext>();
    
            var connectionString = configuration.GetConnectionString("DefaultConnection");
    
            builder.UseSqlite(connectionString);
    
            return new TodoListContext(builder.Options);
        }
    }
}