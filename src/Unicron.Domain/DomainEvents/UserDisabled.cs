using System;

namespace Unicron.Users.Domain.DomainEvents
{
    public class UserDisabled 
    {
        public Guid id { get; protected set; }

        public UserDisabled(Guid id)
        {
            this.id = id;
        }

        protected UserDisabled()
        {
            
        }
    }
}