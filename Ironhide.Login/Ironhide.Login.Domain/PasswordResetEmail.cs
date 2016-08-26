using System;

namespace Ironhide.Login.Domain
{
    public class PasswordResetEmail
    {
        public PasswordResetEmail(string baseUrl, Guid token)
        {
            ResetUrl = string.Format("{0}/#/reset-password?token={1}", baseUrl, token.ToString().ToUpper());
        }

        public string ResetUrl { get; private set; }
    }
}