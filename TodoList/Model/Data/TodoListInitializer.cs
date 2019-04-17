using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TodoList.Core.Data;
using TodoList.Model.Data.Security;

namespace TodoList.Model.Data
{
    public class TodoListInitializer : IInitializer
    {
        private TodoListContext _context;
        private UserManager<CoreIdentityUser> _userManager;

        public TodoListInitializer(TodoListContext context, UserManager<CoreIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            var email = "fmacedoo@dispostable.com";
            var password = "ohmypass";
            var name = "Filipe Macêdo";
            
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new CoreIdentityUser { UserName = email, Email = email, Name = name };
                
                var userResult = await _userManager.CreateAsync(user, password);
                if (!userResult.Succeeded) {
                    throw new InvalidOperationException(userResult.Errors.Select(e => e.Description).Aggregate((a, b) => a + " - " + b));
                }

                var claimResult = await _userManager.AddClaimAsync(user, TodoListClaims.Roles.Common);
                if (!claimResult.Succeeded) {
                    throw new InvalidOperationException(userResult.Errors.Select(e => e.Description).Aggregate((a, b) => a + " - " + b));
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}