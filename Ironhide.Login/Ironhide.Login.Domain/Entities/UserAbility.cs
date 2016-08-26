using System;

namespace Ironhide.Login.Domain.Entities
{
    public class UserAbility : Entity
    {
        protected UserAbility()
        {
        }

        public UserAbility(Guid id, string description)
        {
            Description = description;
            Id = id;
        }

        public virtual string Description { get; protected set; }
    }
}