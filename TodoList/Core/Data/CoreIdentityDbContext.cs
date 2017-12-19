using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TodoList.Core.Data
{
    public class CoreIdentityDbContext : IdentityDbContext<CoreIdentityUser, CoreRole, string>
    {
        public CoreIdentityDbContext(DbContextOptions options) : base(options) { }

        protected CoreIdentityDbContext() { }

        protected new virtual void OnModelCreating(ModelBuilder builder)
        {  
            base.OnModelCreating(builder);
        }
    }
}