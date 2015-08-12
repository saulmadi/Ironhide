using AcklenAvenue.Commands;
using Unicron.Users.Domain.Application.Commands;
using Unicron.Users.Domain.DomainEvents;
using Unicron.Users.Domain.Entities;
using Unicron.Users.Domain.Services;

namespace Unicron.Users.Domain.Application.CommandHandlers
{
    public class UserGoogleCreator : ICommandHandler<CreateGoogleLoginUser>
    {
        readonly IWriteableRepository _writeableRepository;

        public UserGoogleCreator(IWriteableRepository writeableRepository)
        {
            _writeableRepository = writeableRepository;
        }

        public void Handle(IUserSession userIssuingCommand, CreateGoogleLoginUser command)
        {
            var userCreated = _writeableRepository.Create(new UserGoogleLogin(command.DisplayName, command.Email, command.Id, command.GivenName, command.FamilyName, command.ImageUrl, command.Url));
            NotifyObservers(new UserGoogleCreated(userCreated.Id, command.Email, command.DisplayName, command.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}