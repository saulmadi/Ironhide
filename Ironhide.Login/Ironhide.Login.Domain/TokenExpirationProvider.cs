using System;
using System.Configuration;

namespace Ironhide.Login.Domain
{
    public class TokenExpirationProvider : ITokenExpirationProvider
    {
        public DateTime GetExpiration(DateTime now)
        {
            int expirationDays = Convert.ToInt32(ConfigurationManager.AppSettings["PasswordExpirationDays"] ?? "15");
            return now.AddDays(expirationDays);
        }
    }
}