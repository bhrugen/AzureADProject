using AzureADB2CWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace AzureADB2CWeb.Helper
{
    public class PermissionAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public string Role { get; set; }
        private IUserService userService;

        public PermissionAttribute(string role)
        {
            Role = role;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            userService = context.HttpContext.RequestServices.GetService<IUserService>();

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = userService.GetUserFromSession();
                var newClaim = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Role,user.UserRole)
                });

                context.HttpContext.User.AddIdentity(newClaim);
                if (user.UserRole == Role)
                {
                    return;
                }

                context.Result = new StatusCodeResult(403);
                context.Result = new RedirectResult("/error/403");
                return;
            }
            context.Result = new StatusCodeResult(401);
            context.Result = new RedirectResult("/error/401");
            return;

        }
    }
}
