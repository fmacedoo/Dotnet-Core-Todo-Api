using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoList.Core.Data;
using TodoList.Core.Security;
using TodoList.Model.Data;

namespace TodoList.Model.Services
{
    public class UserResolverService : IUserResolverService
    {
        TodoListContext _context;
        UserManager<CoreIdentityUser> _userManager;

        public UserResolverService(TodoListContext context, UserManager<CoreIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<CoreIdentityUser> Resolve(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, password))
                {
                    var userWithClaims = await _context.Users
                        .Include(o => o.Claims)
                        .Where(o => o.Id == user.Id)
                        .SingleOrDefaultAsync();

                    return userWithClaims;
                }
            }

            // Credentials are invalid, or account doesn't exist
            return null;
        }
    }
}