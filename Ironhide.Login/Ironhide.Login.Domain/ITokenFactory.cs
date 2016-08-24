using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Domain
{
    public interface ITokenFactory
    {
        string Create(User executor);
    }
}