using System;

namespace Ironhide.Users.Domain.Entities
{
    public class PasswordResetToken : IEntity
    {
        protected PasswordResetToken()
        {
        }

        public PasswordResetToken(Guid token, Guid userId, DateTime created)
        {
            Id = token;
            UserId = userId;
            Created = created;
        }

        public virtual Guid UserId { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public virtual Guid Id { get; protected set; }
    }
}