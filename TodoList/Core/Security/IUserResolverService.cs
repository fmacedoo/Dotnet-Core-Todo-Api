using System.Threading.Tasks;
using TodoList.Core.Data;

namespace TodoList.Core.Security
{
    public interface IUserResolverService
    {
        Task<CoreIdentityUser> Resolve(string userName, string password);
    }
}