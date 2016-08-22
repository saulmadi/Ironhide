using System;

namespace Ironhide.Users.Domain
{
    public interface ITokenExpirationProvider
    {
        DateTime GetExpiration(DateTime now);
    }
}