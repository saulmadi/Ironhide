using System;

namespace Ironhide.Login.Domain
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}