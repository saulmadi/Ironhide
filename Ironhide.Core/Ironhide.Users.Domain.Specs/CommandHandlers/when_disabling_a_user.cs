using AcklenAvenue.Commands;
using FizzWare.NBuilder;
using Ironhide.Users.Domain.Application.CommandHandlers;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Services;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Users.Domain.Specs.CommandHandlers
{
    public class when_disabling_a_user
    {
        static DisableUser _command;
        static ISampleRepository _readOnlyRepository;
        static IEventedCommandHandler<IUserSession, DisableUser> _handler;
        static UserDisabled _expectedEvent;
        static object _eventRaised;
        static User _userDisable;

        Establish context =
            () =>
            {
                _command = Builder<DisableUser>.CreateNew().Build();

                _userDisable = Builder<User>.CreateNew().With(user => user.Id, _command.id).Build();
                _readOnlyRepository = Mock.Of<ISampleRepository>();

                Mock.Get(_readOnlyRepository)
                    .Setup(repository => repository.GetById<User>(_command.id))
                    .ReturnsAsync(_userDisable);

                _handler = new UserDisabler(_readOnlyRepository);

                _expectedEvent = new UserDisabled(_command.id);
                _handler.NotifyObservers += x => _eventRaised = x;
            };

        Because of =
            () => _handler.Handle(Mock.Of<IUserSession>(), _command);

        It should_enable_user =
            () => _userDisable.IsActive.ShouldBeFalse();

        It should_throw_the_expected_event =
            () => _eventRaised.ShouldBeLike(_expectedEvent);
    }
}