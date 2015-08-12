using System;

namespace Unicron.Users.Domain
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}