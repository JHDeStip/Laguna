using System.Security.Claims;

namespace JhDeStip.Laguna.Server.Services
{
    public interface IUserService
    {
        ClaimsPrincipal GetUserPrincipalByApiKey(string apiKey);
    }
}