using AcklenAvenue.Commands;
using Nancy;
using Unicron.Users.Domain.Entities;
using Unicron.Web.Api.Infrastructure.Authentication;
using Unicron.Web.Api.Infrastructure.Exceptions;

namespace Unicron.Web.Api.Infrastructure
{
    public static class IdentityExtentions
    {
        public static IUserSession UserSession(this NancyModule module)
        {
            var user = module.Context.CurrentUser as LoggedInUserIdentity;
            if (user == null) throw new NoCurrentUserException();
            return user.UserSession;
        }

        public static UserLoginSession UserLoginSession(this NancyModule module)
        {
            var user = module.Context.CurrentUser as LoggedInUserIdentity;
            if (user == null || user.UserSession.GetType() != typeof(UserLoginSession)) throw new NoCurrentUserException();
            return (UserLoginSession) user.UserSession;
        }
    }
}