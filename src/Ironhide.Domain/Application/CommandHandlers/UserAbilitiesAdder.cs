using System.Linq;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class UserAbilitiesAdder: ICommandHandler<AddAbilitiesToUser>
    {
        public IWriteableRepository WriteableRepository { get; private set; }
        public IReadOnlyRepository ReadOnlyRepository { get; private set; }

        public UserAbilitiesAdder(IWriteableRepository writeableRepository, IReadOnlyRepository readOnlyRepository)
        {
            WriteableRepository = writeableRepository;
            ReadOnlyRepository = readOnlyRepository;
        }

        public void Handle(IUserSession userIssuingCommand, AddAbilitiesToUser command)
        {
            //TODO validate duplicate abilities
            var user = ReadOnlyRepository.GetById<User>(command.UserId);
            var abilities = command.AbilitiesID.ToList().Select(x => ReadOnlyRepository.GetById<UserAbility>(x));

            abilities.ToList().ForEach(user.AddAbility);

            WriteableRepository.Update(user);
            NotifyObservers(new UserAbilitiesAdded(user.Id, abilities.Select(x=>x.Id)));
        }

        public event DomainEvent NotifyObservers;
    }
}