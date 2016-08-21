using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserRoleAdder : IEventedCommandHandler<IUserSession, AddRoleToUser>
    {
        readonly IRoleRepository _roleReadRepo;
        readonly IUserRepository<User> _userReadRepo;
        readonly IWriteableRepository _writeableRepository;

        public UserRoleAdder(IWriteableRepository writeableRepository, IUserRepository<User> userReadRepo,
            IRoleRepository roleReadRepo)
        {
            _writeableRepository = writeableRepository;
            _userReadRepo = userReadRepo;
            _roleReadRepo = roleReadRepo;
        }

        public async Task Handle(IUserSession userIssuingCommand, AddRoleToUser command)
        {
            User user = await _userReadRepo.GetById(command.UserId);
            Role role = await _roleReadRepo.GetById(command.RolId);

            user.AddRole(role);

            await _writeableRepository.Update(user);
            NotifyObservers(new UserRoleAdded(user.Id, role.Id));
        }

        public event DomainEvent NotifyObservers;
    }
}