using AcklenAvenue.Commands;
using Nancy.Security;

namespace Ironhide.Web
{
    public interface IUserSessionFactory
    {
        IUserSession Create(IUserIdentity currentUser);
    }
}