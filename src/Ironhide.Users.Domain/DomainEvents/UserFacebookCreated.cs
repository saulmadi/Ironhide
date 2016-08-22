using System;

namespace Ironhide.Users.Domain.DomainEvents
{
    public class UserFacebookCreated : UserCreated
    {
        public UserFacebookCreated(Guid id, string email, string name, string facebookId) : base(id, email, name)
        {
            FacebookId = facebookId;
        }

        public string FacebookId { get; protected set; }
    }
}