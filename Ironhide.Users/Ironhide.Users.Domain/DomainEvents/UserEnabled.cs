using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class UserEnabled
    {
        public UserEnabled(Guid id)
        {
            this.id = id;
        }

        protected UserEnabled()
        {
        }

        public Guid id { get; protected set; }
    }
}