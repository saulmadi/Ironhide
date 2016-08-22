using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class UserRoleAdded
    {
        public UserRoleAdded(Guid userId, Guid rolId)
        {
            UserId = userId;
            RolId = rolId;
        }

        public Guid UserId { get; protected set; }
        public Guid RolId { get; protected set; }
    }
}