using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoList.Core.Security
{
    public class TokenAuthorizationFilter : IAuthorizationFilter
    {
        TokenHelper _userIdentity;

        public TokenAuthorizationFilter(TokenHelper userIdentity)
        {
            _userIdentity = userIdentity;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!_userIdentity.Authorize(context)) {
                // Send an UnauthorizedResult to block the request right here.
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            }
        }
    }
}
