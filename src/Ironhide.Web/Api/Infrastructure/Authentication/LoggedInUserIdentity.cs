using System;
using System.Collections.Generic;
using System.Linq;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Entities;
using Nancy.Security;
using System.Security.Claims;
using NHibernate.Util;

namespace Ironhide.Web.Api.Infrastructure.Authentication
{
    public class LoggedInUserIdentity : IUserIdentity
    {
        public LoggedInUserIdentity(List<Claim> claims)
        {
            this.JwTokenClaims = claims;
            this.Claims = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);

            var userName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            if (userName != null)
            {
                this.UserName = userName.Value;
            }
        }

        public IEnumerable<Claim> JwTokenClaims { get; private set; } 
        public IEnumerable<string> Claims { get; private set; }

        public string UserName { get; private set; }
    }
}