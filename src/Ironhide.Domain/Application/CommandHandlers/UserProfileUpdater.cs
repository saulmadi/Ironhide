using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserProfileUpdater : IEventedCommandHandler<IUserSession, UpdateUserProfile>
    {
        readonly IUserRepository<User> _readonlyRepo;
        readonly IWriteableRepository _writeableRepository;

        public UserProfileUpdater(IUserRepository<User> readonlyRepo, IWriteableRepository writeableRepository)
        {
            _readonlyRepo = readonlyRepo;
            _writeableRepository = writeableRepository;
        }

        public async Task Handle(IUserSession userIssuingCommand, UpdateUserProfile command)
        {
            User user = await _readonlyRepo.GetById(command.Id);
            user.ChangeName(command.Name);
            user.ChangeEmailAddress(command.Email);
            await _writeableRepository.Update(user);
            NotifyObservers(new UserProfileUpdated(user.Id, command.Name, command.Email));
        }

        public event DomainEvent NotifyObservers;
    }
}