using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TodoList.Model.Data.Security
{
    public class TodoListClaims
    {
        public static class Roles
        {
            public const string Type = "Role";
            public readonly static Claim Common = new Claim("Role", "Common");
            
            public static IEnumerable<Claim> GetClaims()
            {
                return new Claim[] {
                    Common
                };
            }

            public static Claim GetClaimByValue(string value)
            {
                return GetClaims().FirstOrDefault(o => o.Value == value);
            }

            public static IEnumerable<string> GetClaimValues()
            {
                return GetClaims().Select(o => o.Value);
            }
        }
    }
}