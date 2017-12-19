using System.Threading.Tasks;

namespace TodoList.Core.Security
{
    public interface ITokenResolverService
    {
        Task<TokenResolved> Resolve(string userName, string password);
    }
}