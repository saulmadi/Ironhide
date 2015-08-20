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
    public class LoggedInUserIdentity : IUserIdentity, IUserSession
    {
        internal static LoggedInUserIdentity CreateVisitorUserIdentity()
        {
            var visitor = new LoggedInUserIdentity();
            return visitor;
        }

        private LoggedInUserIdentity()
        {
            this.JwTokenClaims = new List<Claim>();
            this.Claims = new List<string>();
        }

        public LoggedInUserIdentity(List<Claim> claims)
        {
            this.JwTokenClaims = claims;
            this.Claims = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);
            this.UserName = claims.First(x => x.Type == ClaimTypes.Name).Value;
            this.Id = Guid.Parse(claims.First(x => x.Type == "jti").Value);
        }

        public IEnumerable<Claim> JwTokenClaims { get; private set; } 
        public IEnumerable<string> Claims { get; private set; }

        public string UserName { get; private set; }

        public Guid Id { get; private set; }
    }
}