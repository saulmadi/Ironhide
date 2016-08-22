using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserDisabler : IEventedCommandHandler<IUserSession, DisableUser>
    {
        readonly IUserRepository _repo;

        public UserDisabler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(IUserSession userIssuingCommand, DisableUser command)
        {
            User user = await _repo.GetById<User>(command.id);
            user.DisableUser();
            await _repo.Update(user);
            NotifyObservers(new UserDisabled(user.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}