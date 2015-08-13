using System;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Exceptions;
using Ironhide.Users.Domain.Services;
using Ironhide.Web.Api.Infrastructure.Authentication;
using Nancy.Security;

namespace Ironhide.Web.Api.Infrastructure.Configuration
{
    public class ApiUserMapper : IApiUserMapper<Guid>
    {
        readonly IReadOnlyRepository _readOnlyRepo;
        readonly ITimeProvider _timeProvider;

        public ApiUserMapper(IReadOnlyRepository readOnlyRepo, ITimeProvider timeProvider)
        {
            _readOnlyRepo = readOnlyRepo;
            _timeProvider = timeProvider;
        }

        #region IApiUserMapper<Guid> Members

        public IUserIdentity GetUserFromAccessToken(Guid token)
        {
            UserLoginSession userLoginSession = GetUserSessionFromToken(token);
            MakeSureTokenHasntExpiredYet(userLoginSession);
            return new LoggedInUserIdentity(userLoginSession);
        }

        #endregion

        UserLoginSession GetUserSessionFromToken(Guid token)
        {
            UserLoginSession userLoginSession;
            try
            {
                userLoginSession = _readOnlyRepo.First<UserLoginSession>(x => x.Id == token);
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