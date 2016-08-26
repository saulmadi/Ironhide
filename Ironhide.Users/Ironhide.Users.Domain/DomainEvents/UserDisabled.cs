using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class UserDisabled
    {
        public UserDisabled(Guid id)
        {
            this.id = id;
        }

        protected UserDisabled()
        {
        }

        public Guid id { get; protected set; }
    }
}