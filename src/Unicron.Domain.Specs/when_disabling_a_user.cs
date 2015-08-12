using AcklenAvenue.Commands;
using FizzWare.NBuilder;
using Machine.Specifications;
using Moq;
using Unicron.Users.Domain.Application.CommandHandlers;
using Unicron.Users.Domain.Application.Commands;
using Unicron.Users.Domain.DomainEvents;
using Unicron.Users.Domain.Entities;
using Unicron.Users.Domain.Services;
using It = Machine.Specifications.It;

namespace Unicron.Domain.Specs
{
    public class when_disabling_a_user
    {
        static DisableUser _command;
        static IWriteableRepository _writeableRepository;
        static IReadOnlyRepository _readOnlyRepository;
        static ICommandHandler<DisableUser> _handler;
        static UserDisabled _expectedEvent;
        static object _eventRaised;
        static User _userDisable;

        Establish context =
            () =>
            {
                _command = Builder<DisableUser>.CreateNew().Build();

                _userDisable = Builder<User>.CreateNew().With(user => user.Id, _command.id).Build();
                _writeableRepository = Mock.Of<IWriteableRepository>();
                _readOnlyRepository = Mock.Of<IReadOnlyRepository>();

                Mock.Get(_readOnlyRepository)
                    .Setup(repository => repository.GetById<User>(_command.id))
                    .Returns(_userDisable);

                _handler = new DisablingUser(_writeableRepository, _readOnlyRepository);

                _expectedEvent = new UserDisabled(_command.id);
                _handler.NotifyObservers += x => _eventRaised = x;
            };

        Because of =
            () => { _handler.Handle(Mock.Of<IUserSession>(), _command); };

        It should_enable_user =
            () => { _userDisable.IsActive.ShouldBeFalse(); };

        It should_throw_the_expected_event =
         () => _eventRaised.ShouldBeLike(_expectedEvent);
    }

   
}
