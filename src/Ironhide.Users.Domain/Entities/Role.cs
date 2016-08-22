using System;

namespace Ironhide.Users.Domain.Entities
{
    public class Role : IEntity
    {
        public Role(Guid id, string description)
        {
            Id = id;
            Description = description;
        }

        protected Role()
        {
        }

        public virtual string Description { get; protected set; }
        public virtual Guid Id { get; protected set; }
    }
}