using System;
using System.Collections.Generic;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Exceptions;
using Ironhide.Users.Domain.Services;
using Ironhide.Web.Api.Infrastructure.Authentication;
using Nancy.Security;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using NHibernate.Mapping;
using NHibernate.Util;

namespace Ironhide.Web.Api.Infrastructure.Configuration
{
    public class ApiUserMapper : IApiUserMapper<string>
    {
        readonly IReadOnlyRepository _readOnlyRepo;
        readonly ITimeProvider _timeProvider;
        readonly IKeyProvider _keyProvider;

        public ApiUserMapper(IReadOnlyRepository readOnlyRepo, ITimeProvider timeProvider, IKeyProvider keyProvider)
        {
            _readOnlyRepo = readOnlyRepo;
            _timeProvider = timeProvider;
            _keyProvider = keyProvider;
        }

        #region IApiUserMapper<Guid> Members

        public IUserIdentity GetUserFromAccessToken(string token)
        {
            IEnumerable<Claim> claims = GetClaimsFromToken(token);
            return new LoggedInUserIdentity(claims.ToList());
        }

        #endregion

        JwtSecurityToken ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new InMemorySymmetricSecurityKey(_keyProvider.GetKey())
            };

            SecurityToken securityToken;
            tokenHandler.ValidateToken(token, validationParameters, out securityToken);

            return (JwtSecurityToken)securityToken;
        }

        IEnumerable<Claim> GetClaimsFromToken(string token)
        {
            try
            {
                var jwtSecurityToken = ValidateToken(token);
                MakeSureTokenHasntExpiredYet(jwtSecurityToken);
                return jwtSecurityToken.Claims;

            }
            catch
            {
                throw new TokenDoesNotExistException();
            }
        }

        void MakeSureTokenHasntExpiredYet(JwtSecurityToken token)
        {
            DateTime expires = token.ValidTo;
            DateTime now = _timeProvider.Now();
            if (expires < now)
            {
                throw new TokenExpiredException();
            }
        }
    }
}