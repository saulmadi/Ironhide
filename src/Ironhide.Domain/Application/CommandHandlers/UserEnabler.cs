using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserEnabler : IEventedCommandHandler<IUserSession, EnableUser>
    {
        readonly IUserRepository<User> _readOnlyRepository;
        readonly IWriteableRepository _writeableRepository;

        public UserEnabler(IWriteableRepository writeableRepository, IUserRepository<User> readOnlyRepository)
        {
            _writeableRepository = writeableRepository;
            _readOnlyRepository = readOnlyRepository;
        }

        public async Task Handle(IUserSession userIssuingCommand, EnableUser command)
        {
            User user = await _readOnlyRepository.GetById(command.id);

            user.EnableUser();

            await _writeableRepository.Update(user);

            NotifyObservers(new UserEnabled(user.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}