using AcklenAvenue.Commands;
using Unicron.Users.Domain.Application.Commands;
using Unicron.Users.Domain.DomainEvents;
using Unicron.Users.Domain.Entities;
using Unicron.Users.Domain.Services;

namespace Unicron.Users.Domain.Application.CommandHandlers
{
    public class UserFacebookCreator : ICommandHandler<CreateFacebookLoginUser>
    {
        readonly IWriteableRepository _writeableRepository;

        public UserFacebookCreator(IWriteableRepository writeableRepository)
        {
            _writeableRepository = writeableRepository;
        }

        public void Handle(IUserSession userIssuingCommand, CreateFacebookLoginUser command)
        {
            var userCreated = _writeableRepository.Create(new UserFacebookLogin(command.name,command.email,command.id,command.firstName,command.lastName,command.imageUrl,command.link));
            NotifyObservers(new UserFacebookCreated(userCreated.Id, command.email, command.name, command.id));
        }

        public event DomainEvent NotifyObservers;
    }
}