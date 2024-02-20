using System;
using Microsoft.AspNetCore.Mvc.Filters;
using TestApiForReview.Infrastructure.Exceptions;
using TestApiForReview.Infrastructure.Models.Identity;

namespace TestApiForReview.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!(context.HttpContext.Items["User"] is User))
                throw new ForbiddenException("User unauthorized!");
        }
    }
}