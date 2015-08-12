using System;

namespace Unicron.Users.Domain.Services
{
    public interface ITimeProvider
    {
        DateTime Now();
    }
}