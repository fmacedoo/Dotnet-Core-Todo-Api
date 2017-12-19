using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoList.Core.Data;
using TodoList.Core.Security;
using TodoList.Model.Data;

namespace TodoList.Model.Services
{
    public class TokenResolverService : ITokenResolverService
    {
        TodoListContext _context;
        UserManager<CoreIdentityUser> _userManager;

        public TokenResolverService(TodoListContext context, UserManager<CoreIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<TokenResolved> Resolve(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, password))
                {
                    var tokenResolved = new TokenResolved();
                    tokenResolved.User = await _context.Users.SingleOrDefaultAsync(o => o.Id == user.Id);
                    tokenResolved.Claims = await _context.UserClaims.Where(o => o.UserId == user.Id)
                        .Select(o => TokenClaim.Parse(o))
                        .ToListAsync();

                    return tokenResolved;
                }
            }

            // Credentials are invalid, or account doesn't exist
            return null;
        }
    }
}