using JhDeStip.Laguna.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using System;
using System.Security.Claims;

using System.Threading.Tasks;

namespace JhDeStip.Laguna.Server.Middleware
{
    public class ApiKeySignInManager
    {
        private const string API_KEY_PARAM_NAME = "key";

        private readonly RequestDelegate _next;
        private readonly IUserService _userService;

        public ApiKeySignInManager(RequestDelegate next, IUserService userService)
        {
            _next = next;
            _userService = userService;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                context.User = _userService.GetUserPrincipalByApiKey(context.Request.Query[API_KEY_PARAM_NAME]);
                //await context.Authentication.SignInAsync("Test", _userService.GetUserPrincipalByApiKey(context.Request.Query[API_KEY_PARAM_NAME]));
            }
            catch (Exception e)
            {
                ;
            }

            await _next.Invoke(context);
        }
    }
}
