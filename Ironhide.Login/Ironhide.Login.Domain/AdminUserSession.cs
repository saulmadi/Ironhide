using System;
using AcklenAvenue.Commands;

namespace Ironhide.Login.Domain
{
    public class AdminUserSession : IUserSession
    {
        public AdminUserSession(string userIdentifier)
        {
            UserIdentifier = userIdentifier;
        }

        public Guid Id { get; protected set; }
        public string UserIdentifier { get; private set; }
    }
}