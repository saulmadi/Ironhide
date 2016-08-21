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
        readonly IUserRepository<User> _repo;
        
        public UserEnabler(IUserRepository<User> repo)
        {
            _repo = repo;
        }

        public async Task Handle(IUserSession userIssuingCommand, EnableUser command)
        {
            User user = await _repo.GetById(command.id);
            user.EnableUser();
            await _repo.Update(user);
            NotifyObservers(new UserEnabled(user.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}