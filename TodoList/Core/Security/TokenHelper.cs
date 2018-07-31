using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using TodoList.Core.Cache;
using TodoList.Core.Data;
using TodoList.Core.Middlewares;

namespace TodoList.Core.Security
{
    public class TokenHelper
    {
        ClaimsPrincipal _claimsPrincipal;

        public TokenHelper(ClaimsPrincipal claimsPrincipal)
        {
            _claimsPrincipal = claimsPrincipal;
        }

        public string GetIdentityUserId()
        {
            return _claimsPrincipal.HasClaim(o => o.Type == ClaimTypes.NameIdentifier) ? 
                _claimsPrincipal.Claims.First(o => o.Type == ClaimTypes.NameIdentifier).Value : null;
        }

        public bool HasClaim(Claim claim)
        {
            return _claimsPrincipal.HasClaim(claim.Type, claim.Value);
        }

        public bool HasClaim(params Claim[] claims)
        {
            return _claimsPrincipal.Claims.Select(o => o.ToString()).Intersect(claims.Select(o => o.ToString())).Any();
        }

        public async Task<string> Build(CoreIdentityUser user, TokenProviderOptions options)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, await options.NonceGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64),
                new Claim("IdentityUserId", user.Id)
            };

            claims.Union(user.Claims.Select(o => new Claim(o.ClaimType, o.ClaimValue)));

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: now,
                expires: now.Add(options.Expiration),
                signingCredentials: options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public bool Authorize(ActionContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor.MethodInfo.CustomAttributes.Any(o => o.AttributeType == typeof(NoTokenAuthAttribute)))
            {
                // Authorization by NoTokenAuth action marked
                return true;
            }

            var authorizationToken = getAuthorizationToken(context.HttpContext.Request);
            if (authorizationToken != null) {
                var jwt = new JwtSecurityToken(authorizationToken);
                if (jwt != null && DateTime.Now < jwt.ValidTo)
                {
                    // Set the jwt claims at the ClaimsPrincipal to future validations
                    var claimsIdentity = new ClaimsIdentity(jwt.Claims);
                    _claimsPrincipal.AddIdentity(claimsIdentity);
                    
                    return true;
                }
            }

            return false;
        }

        // Catches the Authorization header content and return the token string
        private string getAuthorizationToken(HttpRequest request)
        {
            var header = request.Headers["Authorization"].FirstOrDefault();
            if (header != null)
            {
                var parts = header.Split(' ');
                if (parts.Length > 1 && parts[0].Equals("token"))
                {
                    return parts[1];
                }
            }

            return null;
        }
    }
}