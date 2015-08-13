using System;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Domain.Services
{
    public class UserSessionFactory : IUserSessionFactory
    {
        readonly ITimeProvider _timeProvider;
        readonly ITokenExpirationProvider _tokenExpirationProvider;
        readonly IIdentityGenerator<Guid> _identityGenerator;
        readonly IWriteableRepository _writeableRepository;

        public UserSessionFactory(IWriteableRepository writeableRepository,
                                  ITimeProvider timeProvider,
                                  IIdentityGenerator<Guid> identityGenerator,
                                  ITokenExpirationProvider tokenExpirationProvider)
        {
            _writeableRepository = writeableRepository;
            _timeProvider = timeProvider;
            _identityGenerator = identityGenerator;
            _tokenExpirationProvider = tokenExpirationProvider;
        }

        #region IUserSessionFactory Members

        public UserLoginSession Create(User executor)
        {
            DateTime dateTime = _tokenExpirationProvider.GetExpiration(_timeProvider.Now());
            Guid token = _identityGenerator.Generate();

            var userSession = new UserLoginSession(token, executor, dateTime);

            _writeableRepository.Create(userSession);

            return userSession;
        }

        #endregion
    }
}