using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace TodoList.Core.Security
{
    public class TokenClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public Claim ToClaim()
        {
            return new Claim(Type, Value);
        }

        public static TokenClaim Parse(IdentityUserClaim<string> o)
        {
            return new TokenClaim { Type = o.ClaimType, Value = o.ClaimValue };
        }
    }
}
