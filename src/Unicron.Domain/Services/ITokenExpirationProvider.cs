using System;

namespace Unicron.Users.Domain.Services
{
    public interface ITokenExpirationProvider
    {
        DateTime GetExpiration(DateTime now);
    }
}