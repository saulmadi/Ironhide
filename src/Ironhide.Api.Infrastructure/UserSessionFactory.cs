using System;
using System.Linq;
using AcklenAvenue.Commands;
using Ironhide.Api.Infrastructure.Authentication;
using Ironhide.Users.Domain;
using Nancy.Security;

namespace Ironhide.Api.Infrastructure
{
    public class UserSessionFactory : IUserSessionFactory
    {
        public IUserSession Create(IUserIdentity currentUser)
        {
            var loggedInUserIdentity = (LoggedInUserIdentity) currentUser;
            if (HasRole(loggedInUserIdentity, "Administrator"))
            {
                Guid userId = GetUserId(loggedInUserIdentity);
                return new AdminUserSession(userId.ToString());
            }
            if (HasRole(loggedInUserIdentity, "Basic"))
            {
                Guid userId = GetUserId(loggedInUserIdentity);
                return new BasicUserSession(userId.ToString());
            }
            return new VisitorSession();
        }

        static bool HasRole(LoggedInUserIdentity loggedInUserIdentity, string administrator)
        {
            return loggedInUserIdentity.Claims.Any(x => x.Equals(administrator));
        }

        static Guid GetUserId(LoggedInUserIdentity loggedInUserIdentity)
        {
            return Guid.Parse(loggedInUserIdentity.JwTokenClaims.First(x => x.Type.Equals("userguid")).Value);
        }
    }
}