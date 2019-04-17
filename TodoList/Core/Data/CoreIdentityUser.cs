using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TodoList.Core.Data
{
    public class CoreIdentityUser : IdentityUser<string>
    {
        public CoreIdentityUser()
        {
        }

        public CoreIdentityUser(string userName) : base(userName)
        {
        }

        public string Name { get; set; }
        public virtual ICollection<CoreUserRole> Roles { get; } = new List<CoreUserRole>();
        public virtual ICollection<CoreUserClaim> Claims { get; } = new List<CoreUserClaim>();
    }
}