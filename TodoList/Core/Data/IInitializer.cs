using System.Threading.Tasks;

namespace TodoList.Core.Data
{
    public interface IInitializer
    {
        Task SeedAsync();
    }
}