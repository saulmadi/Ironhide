﻿using AcklenAvenue.Commands;
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
    public class when_enabling_a_user
    {
        static EnableUser _command;
        static IUserRepository _userRepo;
        static IEventedCommandHandler<IUserSession, EnableUser> _handler;
        static UserEnabled _expectedEvent;
        static object _eventRaised;
        static User _userEnabled;

        Establish context =
            () =>
            {
                _command = Builder<EnableUser>.CreateNew().Build();

                _userEnabled = Builder<User>.CreateNew().With(user => user.Id, _command.id).Build();
                _userRepo = Mock.Of<IUserRepository>();

                Mock.Get(_userRepo)
                    .Setup(repository => repository.GetById<User>(_command.id))
                    .ReturnsAsync(_userEnabled);

                _handler = new UserEnabler(_userRepo);

                _expectedEvent = new UserEnabled(_command.id);
                _handler.NotifyObservers += x => _eventRaised = x;
            };

        Because of =
            () => _handler.Handle(Mock.Of<IUserSession>(), _command);

        It should_enable_user =
            () => _userEnabled.IsActive.ShouldBeTrue();

        It should_throw_the_expected_event =
            () => _eventRaised.ShouldBeLike(_expectedEvent);
    }
}