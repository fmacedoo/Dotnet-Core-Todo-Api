using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using TodoList.Core.Cache;

namespace TodoList.Core.Security
{
    public class TokenHelper
    {
        ICacheProvider _cacheProvider;
        ClaimsPrincipal _claimsPrincipal;

        public TokenHelper(ICacheProvider cacheProvider, ClaimsPrincipal claimsPrincipal)
        {
            _cacheProvider = cacheProvider;
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

        private void setClaims(TokenResolved identityUser)
        {
            var claimsIdentity = new ClaimsIdentity(identityUser.Claims.Select(o => o.ToClaim()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identityUser.User.Id));
            _claimsPrincipal.AddIdentity(claimsIdentity);
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
                var token = _cacheProvider.Get<Token>(authorizationToken);
                if (token != null && DateTime.Now < token.CreatedAt.AddSeconds(token.ExpiresIn))
                {
                    setClaims(token.User);
                    return true;
                }
            }

            return false;
        }

        public void Authenticate(Token token)
        {
            _cacheProvider.Store(token.Value, token);
        }

        public void Logoff(HttpRequest request)
        {
            var authorizationToken = getAuthorizationToken(request);
            _cacheProvider.Delete<Token>(authorizationToken);
        }

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