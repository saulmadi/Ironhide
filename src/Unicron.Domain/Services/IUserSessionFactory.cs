using Unicron.Users.Domain.Entities;

namespace Unicron.Users.Domain.Services
{
    public interface IUserSessionFactory
    {
        UserLoginSession Create(User executor);
    }
}