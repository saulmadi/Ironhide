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
        readonly IUserRepository<User> _repo;
        
        public UserProfileUpdater(IUserRepository<User> repo)
        {
            _repo = repo;            
        }

        public async Task Handle(IUserSession userIssuingCommand, UpdateUserProfile command)
        {
            User user = await _repo.GetById(command.Id);
            user.ChangeName(command.Name);
            user.ChangeEmailAddress(command.Email);
            await _repo.Update(user);
            NotifyObservers(new UserProfileUpdated(user.Id, command.Name, command.Email));
        }

        public event DomainEvent NotifyObservers;
    }
}