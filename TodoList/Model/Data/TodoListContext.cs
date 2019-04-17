using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoList.Core.Data;

namespace TodoList.Model.Data
{
    public class TodoListContext : CoreIdentityDbContext
    {
        public TodoListContext(DbContextOptions options) : base(options) { }

        protected TodoListContext() { }
    }
}