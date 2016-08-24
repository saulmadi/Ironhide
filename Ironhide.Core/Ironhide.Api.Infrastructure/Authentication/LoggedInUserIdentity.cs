using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Nancy.Security;

namespace Ironhide.Api.Infrastructure.Authentication
{
    public class LoggedInUserIdentity : IUserIdentity
    {
        public LoggedInUserIdentity(List<Claim> claims)
        {
            JwTokenClaims = claims;
            Claims = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);

            Claim userName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            if (userName != null)
            {
                UserName = userName.Value;
            }
        }

        public IEnumerable<Claim> JwTokenClaims { get; private set; }
        public IEnumerable<string> Claims { get; }

        public string UserName { get; }
    }
}