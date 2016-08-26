using System;
using AcklenAvenue.Commands;

namespace Ironhide.App.Domain
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