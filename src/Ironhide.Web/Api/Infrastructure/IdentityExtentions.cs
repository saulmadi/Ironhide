using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Entities;
using Ironhide.Web.Api.Infrastructure.Authentication;
using Ironhide.Web.Api.Infrastructure.Exceptions;
using Nancy;

namespace Ironhide.Web.Api.Infrastructure
{
    public static class IdentityExtentions
    {
        public static IUserSession UserSession(this NancyModule module)
        {
            var user = module.Context.CurrentUser as LoggedInUserIdentity;
            if (user == null) throw new NoCurrentUserException();
            return user;
        }

        public static LoggedInUserIdentity UserIdentity(this NancyModule module)
        {
            var user = module.Context.CurrentUser as LoggedInUserIdentity;
            if (user == null) throw new NoCurrentUserException();
            return user;
        }
    }
}