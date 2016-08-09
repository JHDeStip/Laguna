using Microsoft.AspNetCore.Builder;

namespace JhDeStip.Laguna.Server.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeySignInManager(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeySignInManager>();
        }

        public static IApplicationBuilder UseSimpleAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SimpleAuthenticationMiddleware>(new SimpleAuthenticationOptions { AuthenticationScheme = "Test", AutomaticAuthenticate = true, AutomaticChallenge = true });
        }
    }
}
