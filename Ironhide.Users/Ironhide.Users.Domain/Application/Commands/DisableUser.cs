using System;

namespace Ironhide.Users.Domain.Application.Commands
{
    public class DisableUser
    {
        public DisableUser(Guid id)
        {
            this.id = id;
        }

        protected DisableUser()
        {
        }

        public Guid id { get; protected set; }
    }
}