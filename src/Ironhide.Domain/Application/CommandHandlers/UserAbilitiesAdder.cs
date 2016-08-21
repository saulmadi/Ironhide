using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserAbilitiesAdder : IEventedCommandHandler<IUserSession, AddAbilitiesToUser>
    {
        readonly IUserAbilityRepository _abilityReadRepo;
        readonly IUserRepository<User> _userReadRepo;
        
        public UserAbilitiesAdder(IUserRepository<User> userReadRepo, IUserAbilityRepository abilityReadRepo)
        {
            _userReadRepo = userReadRepo;
            _abilityReadRepo = abilityReadRepo;
        }

        public async Task Handle(IUserSession userIssuingCommand, AddAbilitiesToUser command)
        {
            User user = await _userReadRepo.GetById(command.UserId);
            var abilities = new List<UserAbility>();
            foreach (Guid abilityId in command.Abilities)
            {
                UserAbility userAbility = await _abilityReadRepo.GetById(abilityId);
                abilities.Add(userAbility);
                user.AddAbility(userAbility);
            }

            await _userReadRepo.Update(user);

            NotifyObservers(new UserAbilitiesAdded(user.Id, abilities.Select(x => x.Id)));
        }

        public event DomainEvent NotifyObservers;
    }
}