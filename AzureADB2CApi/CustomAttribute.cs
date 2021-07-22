using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Net.Http.Headers;

namespace AzureADB2CApi
{
    public class CustomAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public string Role { get; set; }
        

        public CustomAttribute(string role)
        {
            Role = role;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var token = context.HttpContext.Request.Headers[HeaderNames.Authorization];
            var tokenRead = token.ToString()?.Replace("Bearer ", "");
            if (!string.IsNullOrWhiteSpace(tokenRead))
            {
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(tokenRead);
                var aud = jwtToken.Claims.First(c => c.Type == "aud").Value;
                if(aud== "0daf8d3e-a9a1-4746-bd9f-796b7af9d344")
                {
                    return;
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }

        }
    }
}
