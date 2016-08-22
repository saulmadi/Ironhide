using System;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Common;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class PasswordResetTokenCreator : IEventedCommandHandler<IUserSession, StartPasswordResetProcess>
    {
        readonly IIdentityGenerator<Guid> _idGenerator;
        readonly IUserRepository<UserEmailLogin> _readOnlyRepository;
        readonly ITimeProvider _timeProvider;
        readonly IPasswordResetTokenRepository _tokenRepo;

        public PasswordResetTokenCreator(IPasswordResetTokenRepository tokenRepo,
            IUserRepository<UserEmailLogin> readOnlyRepository, ITimeProvider timeProvider,
            IIdentityGenerator<Guid> idGenerator)
        {
            _tokenRepo = tokenRepo;
            _readOnlyRepository = readOnlyRepository;
            _timeProvider = timeProvider;
            _idGenerator = idGenerator;
        }

        public async Task Handle(IUserSession userIssuingCommand, StartPasswordResetProcess command)
        {
            UserEmailLogin user = await _readOnlyRepository.First(x => x.Email == command.Email);
            Guid tokenId = _idGenerator.Generate();
            await _tokenRepo.Create(new PasswordResetToken(tokenId, user.Id, _timeProvider.Now()));
            NotifyObservers(new PasswordResetTokenCreated(tokenId, user.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}