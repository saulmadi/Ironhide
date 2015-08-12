using System;
using AcklenAvenue.Commands;

namespace Unicron.Users.Domain
{
    public class VisitorSession : IUserSession
    {
        #region IUserSession Members

        public Guid Id { get; private set; }

        #endregion
    }
}