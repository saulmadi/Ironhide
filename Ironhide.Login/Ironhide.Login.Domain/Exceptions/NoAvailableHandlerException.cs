using System;

namespace Ironhide.Login.Domain.Exceptions
{
    public class NoAvailableHandlerException : Exception
    {
        public NoAvailableHandlerException(Type commandType)
            : base(string.Format("No command handler was available for the command type {0}.", commandType))
        {
        }
    }
}