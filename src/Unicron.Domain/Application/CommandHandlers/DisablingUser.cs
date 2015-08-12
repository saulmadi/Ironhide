using AcklenAvenue.Commands;
using Unicron.Users.Domain.Application.Commands;
using Unicron.Users.Domain.DomainEvents;
using Unicron.Users.Domain.Entities;
using Unicron.Users.Domain.Services;

namespace Unicron.Users.Domain.Application.CommandHandlers
{
    public  class DisablingUser : ICommandHandler<DisableUser>
    {
        public IWriteableRepository writeableRepository { get; private set; }
        public IReadOnlyRepository readOnlyRepository { get; private set; }

        public DisablingUser(IWriteableRepository writeableRepository, IReadOnlyRepository readOnlyRepository)
        {
            this.writeableRepository = writeableRepository;
            this.readOnlyRepository = readOnlyRepository;
        }

        public void Handle(IUserSession userIssuingCommand, DisableUser command)
        {
            var user = readOnlyRepository.GetById<User>(command.id);
            user.DisableUser();

            writeableRepository.Update(user);

            NotifyObservers(new UserDisabled(user.Id));


        }

        public event DomainEvent NotifyObservers;
    }
}