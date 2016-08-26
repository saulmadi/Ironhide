using System;

namespace Ironhide.App.Domain
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}