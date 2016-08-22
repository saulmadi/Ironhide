using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using Ironhide.Api.Infrastructure.Authentication;
using Ironhide.Api.Infrastructure.RestExceptions;
using Ironhide.Common;
using Ironhide.Users.Domain;
using Nancy.Security;

namespace Ironhide.Api.Infrastructure.Configuration
{
    public class ApiUserMapper : IApiUserMapper<string>
    {
        readonly IKeyProvider _keyProvider;
        readonly ITimeProvider _timeProvider;

        public ApiUserMapper(ITimeProvider timeProvider, IKeyProvider keyProvider)
        {
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

        ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
                                       {
                                           ValidateAudience = false,
                                           ValidateIssuer = false,
                                           IssuerSigningKey = new InMemorySymmetricSecurityKey(_keyProvider.GetKey())
                                       };

            SecurityToken securityToken;
            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

            var jwtSecurityToken = (JwtSecurityToken) securityToken;
            MakeSureTokenHasntExpiredYet(jwtSecurityToken);
            return claimsPrincipal;
        }

        IEnumerable<Claim> GetClaimsFromToken(string token)
        {
            try
            {
                ClaimsPrincipal claimsPrincipal = ValidateToken(token);
                return claimsPrincipal.Claims;
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