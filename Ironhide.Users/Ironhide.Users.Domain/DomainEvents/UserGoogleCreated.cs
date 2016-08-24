using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class UserGoogleCreated : UserCreated
    {
        public UserGoogleCreated(Guid id, string email, string displayName, string googleId)
            : base(id, email, displayName)
        {
            GoogleId = googleId;
        }

        public string GoogleId { get; set; }
    }
}