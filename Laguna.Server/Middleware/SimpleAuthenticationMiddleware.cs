using JhDeStip.Laguna.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace JhDeStip.Laguna.Server.Middleware
{
    public class SimpleAuthenticationMiddleware : AuthenticationMiddleware<SimpleAuthenticationOptions>
    {

        public SimpleAuthenticationMiddleware(RequestDelegate next, IOptions<SimpleAuthenticationOptions> options, ILoggerFactory loggerFactory, UrlEncoder encoder) : base(next, options, loggerFactory, encoder)
        {
            options.Value.AuthenticationScheme = "Test";
            options.Value.AutomaticAuthenticate = true;
            options.Value.AutomaticChallenge = false;
        }

        protected override AuthenticationHandler<SimpleAuthenticationOptions> CreateHandler()
        {
            return new SimpleAuthenticationHandler();
        }
    }
}
