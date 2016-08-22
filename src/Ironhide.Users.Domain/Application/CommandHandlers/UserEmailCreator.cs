using System;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserEmailCreator : IEventedCommandHandler<IUserSession, CreateEmailLoginUser>
    {
        readonly IUserAbilityRepository _abilityReadRepo;
        readonly IRoleRepository _roleReadRepo;
        readonly IUserRepository _userRepo;

        public UserEmailCreator(IRoleRepository roleReadRepo,
            IUserAbilityRepository abilityReadRepo, IUserRepository userRepo)
        {
            _roleReadRepo = roleReadRepo;
            _abilityReadRepo = abilityReadRepo;
            _userRepo = userRepo;
        }

        #region ICommandHandler Members

        public async Task Handle(IUserSession userIssuingCommand, CreateEmailLoginUser command)
        {
            var userCreated = new UserEmailLogin(Guid.NewGuid(), command.Name, command.Email, command.EncryptedPassword,
                command.PhoneNumber);

            foreach (UserAbility ability in command.Abilities)
            {
                UserAbility userAbility = await _abilityReadRepo.GetById(ability.Id);
                userCreated.AddAbility(userAbility);
            }

            Role basicRole = await _roleReadRepo.First(x => x.Description == "Basic");
            userCreated.AddRole(basicRole);
            User userSaved = await _userRepo.Create(userCreated);

            NotifyObservers(new UserEmailCreated(userSaved.Id, command.Email, command.Name, command.PhoneNumber));
        }

        public event DomainEvent NotifyObservers;

        #endregion
    }
}