using Nancy.Security;

namespace Unicron.Web.Api.Infrastructure.Authentication
{
    public interface IApiUserMapper<in T>
    {
        IUserIdentity GetUserFromAccessToken(T token);
    }
}