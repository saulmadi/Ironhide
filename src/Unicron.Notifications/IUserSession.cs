using System;

namespace Starscream.Notifications
{
    public interface IUserSession
    {
        Guid Id { get; }
    }
}