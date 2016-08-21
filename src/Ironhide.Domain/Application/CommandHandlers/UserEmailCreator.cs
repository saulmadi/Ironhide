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
        readonly IWriteableRepository _writeableRepository;

        public UserEmailCreator(IWriteableRepository writeableRepository, IRoleRepository roleReadRepo,
            IUserAbilityRepository abilityReadRepo)
        {
            _writeableRepository = writeableRepository;
            _roleReadRepo = roleReadRepo;
            _abilityReadRepo = abilityReadRepo;
        }

        #region ICommandHandler Members

        public async Task Handle(IUserSession userIssuingCommand, CreateEmailLoginUser command)
        {
            var userCreated = new UserEmailLogin(Guid.NewGuid(), command.Name, command.Email, command.EncryptedPassword,
                command.PhoneNumber);

            foreach (UserAbility ability in command.abilities)
            {
                UserAbility userAbility = await _abilityReadRepo.GetById(ability.Id);
                userCreated.AddAbility(userAbility);
            }

            Role basicRole = await _roleReadRepo.First(x => x.Description == "Basic");
            userCreated.AddRole(basicRole);
            UserEmailLogin userSaved = await _writeableRepository.Create(userCreated);

            NotifyObservers(new UserEmailCreated(userSaved.Id, command.Email, command.Name, command.PhoneNumber));
        }

        public event DomainEvent NotifyObservers;

        #endregion
    }
}