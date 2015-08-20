using System;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Exceptions;
using Ironhide.Users.Domain.Services;
using Ironhide.Web.Api.Infrastructure.Authentication;
using Nancy.Security;
using System.IdentityModel.Tokens;

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
            UserLoginSession userLoginSession = GetUserSessionFromToken(token);
            MakeSureTokenHasntExpiredYet(userLoginSession);
            return new LoggedInUserIdentity(userLoginSession);
        }

        #endregion

        UserLoginSession GetUserSessionFromToken(string token)
        {
            UserLoginSession userLoginSession;
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new InMemorySymmetricSecurityKey(_keyProvider.GetKey())
                };

                SecurityToken securityToken;
                tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                var jwtSecurityToken = (JwtSecurityToken) securityToken;

                var guid = Guid.Parse(jwtSecurityToken.Id);

                userLoginSession = _readOnlyRepo.First<UserLoginSession>(x => x.Id == guid);
            }
            catch (ItemNotFoundException<UserLoginSession> e)
            {
                throw new TokenDoesNotExistException();
            }
            return userLoginSession;
        }

        void MakeSureTokenHasntExpiredYet(UserLoginSession userLoginSession)
        {
            DateTime expires = userLoginSession.Expires;
            DateTime now = _timeProvider.Now();
            if (expires < now)
            {
                throw new TokenExpiredException();
            }
        }
    }
}