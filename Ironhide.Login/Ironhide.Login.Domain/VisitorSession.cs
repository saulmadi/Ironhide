using System;
using AcklenAvenue.Commands;

namespace Ironhide.Login.Domain
{
    public class VisitorSession : IUserSession
    {
        #region IUserSession Members

        public Guid Id { get; protected set; }
        public string UserIdentifier { get; protected set; }

        #endregion
    }
}