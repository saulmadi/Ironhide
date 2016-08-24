using System;

namespace Ironhide.Users.Domain.Application.Commands
{
    public class AddRoleToUser
    {
        protected AddRoleToUser()
        {
        }

        public AddRoleToUser(Guid userId, Guid rolId)
        {
            UserId = userId;
            RolId = rolId;
        }

        public Guid UserId { get; protected set; }
        public Guid RolId { get; protected set; }
    }
}