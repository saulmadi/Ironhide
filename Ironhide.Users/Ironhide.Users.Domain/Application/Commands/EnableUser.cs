using System;

namespace Ironhide.Users.Domain.Application.Commands
{
    public class EnableUser
    {
        public EnableUser(Guid id)
        {
            this.id = id;
        }

        protected EnableUser()
        {
        }

        public Guid id { get; protected set; }
    }
}