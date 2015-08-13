using System;

namespace Ironhide.Web.Api.Infrastructure.Exceptions
{
    public class UserInputPropertyMissingException : Exception
    {
        public UserInputPropertyMissingException(string parameterName)
            : base(string.Format("The json body property '{0}' was missing and is required.", parameterName))
        {

        }
    }
}