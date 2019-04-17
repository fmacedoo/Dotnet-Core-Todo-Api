using Microsoft.AspNetCore.Identity;

namespace TodoList.Core.Data
{
    public class CoreUserRole : IdentityUserRole<string>
    {
        public CoreUserRole() : base()
        {
        }
    }
}