using System;

namespace Ironhide.Login.Domain
{
    public interface ITokenExpirationProvider
    {
        DateTime GetExpiration(DateTime now);
    }
}