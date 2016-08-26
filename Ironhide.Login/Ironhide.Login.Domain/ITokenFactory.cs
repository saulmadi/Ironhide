using Ironhide.Login.Domain.Entities;

namespace Ironhide.Login.Domain
{
    public interface ITokenFactory
    {
        string Create(User executor);
    }
}