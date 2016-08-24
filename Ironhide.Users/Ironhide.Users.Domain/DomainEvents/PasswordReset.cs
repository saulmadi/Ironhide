using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class PasswordReset
    {
        public PasswordReset(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}