using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Domain.Services
{
    public interface IUserSessionFactory
    {
        UserLoginSession Create(User executor);
    }
}