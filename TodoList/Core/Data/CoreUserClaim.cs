using Microsoft.AspNetCore.Identity;

namespace TodoList.Core.Data
{
    public class CoreUserClaim : IdentityUserClaim<string>
    {
        public CoreUserClaim() : base()
        {
        }

        public CoreUserClaim(string claimType, string claimValue) : base()
        {
            this.ClaimType = claimType;
            this.ClaimValue = claimValue;
        }
    }
}