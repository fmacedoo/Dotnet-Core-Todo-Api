
using System.Collections.Generic;
using System.Security.Claims;
using TodoList.Core.Data;

namespace TodoList.Core.Security
{
    public class TokenResolved
    {
        public CoreIdentityUser User { get; set; }
        public List<TokenClaim> Claims { get; set; }
    }
}
