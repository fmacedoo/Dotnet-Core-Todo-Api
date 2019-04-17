using Microsoft.AspNetCore.Identity;

namespace TodoList.Core.Data
{
    public class CoreRole : IdentityRole<string>
    {
        public CoreRole() : base()
        {
        }
    }
}