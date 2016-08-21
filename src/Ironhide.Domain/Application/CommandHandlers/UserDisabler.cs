using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserDisabler : IEventedCommandHandler<IUserSession, Commands.DisableUser>
    {
        readonly IUserRepository<User> _readOnlyRepository;
        readonly IWriteableRepository _writeableRepository;

        public UserDisabler(IWriteableRepository writeableRepository, IUserRepository<User> readOnlyRepository)
        {
            _writeableRepository = writeableRepository;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task Handle(IUserSession userIssuingCommand, Commands.DisableUser command)
        {
            User user = await _readOnlyRepository.GetById(command.id);
            user.DisableUser();

            await _writeableRepository.Update(user);

            NotifyObservers(new UserDisabled(user.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}