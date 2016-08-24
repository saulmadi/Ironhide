using System;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserFacebookCreator : IEventedCommandHandler<IUserSession, CreateFacebookLoginUser>
    {
        readonly IUserRepository _writeableRepository;

        public UserFacebookCreator(IUserRepository writeableRepository)
        {
            _writeableRepository = writeableRepository;
        }

        public async Task Handle(IUserSession userIssuingCommand, CreateFacebookLoginUser command)
        {
            User userCreated =
                await
                    _writeableRepository.Create(new UserFacebookLogin(Guid.NewGuid(), command.name, command.email,
                        command.id, command.firstName, command.lastName, command.imageUrl, command.link));

            NotifyObservers(new UserFacebookCreated(userCreated.Id, command.email, command.name, command.id));
        }

        public event DomainEvent NotifyObservers;
    }
}