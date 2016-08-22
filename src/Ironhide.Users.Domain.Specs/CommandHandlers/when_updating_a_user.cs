﻿using System;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.CommandHandlers;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Services;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Users.Domain.Specs.CommandHandlers
{
    public class when_updating_a_user
    {
        static IEventedCommandHandler<IUserSession, UpdateUserProfile> _handler;
        static IUserRepository _userRepo;
        static User _user;
        static object _eventRaised;
        static object _expectedEvent;
        static UpdateUserProfile _command;

        Establish context =
            () =>
            {
                _userRepo = Mock.Of<IUserRepository>();
                _handler = new UserProfileUpdater(_userRepo);
                _user = new User(Guid.NewGuid(), "Test User", "test@email.com");

                _command = new UpdateUserProfile(_user.Id, "Test User Updated", "updated@email.com");

                Mock.Get(_userRepo).Setup(x => x.GetById<User>(_user.Id))
                    .ReturnsAsync(_user);

                _handler.NotifyObservers += x => _eventRaised = x;
                _expectedEvent = new UserProfileUpdated(_user.Id, _command.Name, _command.Email);
            };

        Because of =
            () =>
                _handler.Handle(new BasicUserSession(Guid.NewGuid().ToString()), _command);

        It should_throw_the_expected_event =
            () => _eventRaised.ShouldBeLike(_expectedEvent);

        It should_update_the_user_email =
            () => _user.Email.ShouldEqual(_command.Email);

        It should_update_the_user_name =
            () => _user.Name.ShouldEqual(_command.Name);
    }
}