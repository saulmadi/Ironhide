using System;
using AcklenAvenue.Commands;

namespace Ironhide.Users.Domain
{
    public class BasicUserSession : IUserSession
    {
        public BasicUserSession(string userIdentifier)
        {
            UserIdentifier = userIdentifier;
        }

        public Guid Id { get; protected set; }
        public string UserIdentifier { get; }
    }
}