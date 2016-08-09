using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JhDeStip.Laguna.Server.Middleware
{
    public class SimpleAuthenticationHandler : AuthenticationHandler<SimpleAuthenticationOptions>
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return await Task.Run(() =>
            {
                AuthenticationProperties authProps = new AuthenticationProperties();
                authProps.IsPersistent = false;
                authProps.RedirectUri = "http://www.google.be";

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("Active", "True", ClaimValueTypes.Boolean));
                var princ = new ClaimsPrincipal(new ClaimsIdentity(claims));
                Request.HttpContext.Authentication.AuthenticateAsync("Test");
               
                authProps.AllowRefresh = false;
                authProps.ExpiresUtc = DateTime.UtcNow.AddDays(1);
                return AuthenticateResult.Success(new AuthenticationTicket(Request.HttpContext.User, authProps, "Test"));
            });
            
        }
    }
}
