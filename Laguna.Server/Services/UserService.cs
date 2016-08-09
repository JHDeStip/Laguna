using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using JhDeStip.Laguna.Server.Models;

namespace JhDeStip.Laguna.Server.Services
{
    public class UserService : IUserService
    {
        private IList<ClaimsPrincipal> _userPrincipals;

        public UserService(IOptions<List<ApiKeyUser>> users)
        {
            _userPrincipals = new List<ClaimsPrincipal>();

            foreach (var user in users.Value)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(nameof(user.Name), user.Name, ClaimValueTypes.String));
                claims.Add(new Claim(nameof(user.ApiKey), user.ApiKey, ClaimValueTypes.String));
                claims.Add(new Claim(nameof(user.Active), user.Active.ToString(), ClaimValueTypes.Boolean));
                foreach (var permission in user.Permissions)
                    claims.Add(new Claim(nameof(user.Permissions), permission.ToString(), ClaimValueTypes.String));

                _userPrincipals.Add(new ClaimsPrincipal(new ClaimsIdentity(claims)));
            }
        }

        public ClaimsPrincipal GetUserPrincipalByApiKey(string apiKey)
        {
            ApiKeyUser user = null;
            var principal = _userPrincipals.Where(x => x.HasClaim(nameof(user.ApiKey), apiKey)).FirstOrDefault();
            if (principal != null)
                return principal;

            return new ClaimsPrincipal();
        }
    }
}
