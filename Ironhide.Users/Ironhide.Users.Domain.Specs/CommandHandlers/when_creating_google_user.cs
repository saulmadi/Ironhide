﻿using System;
using AcklenAvenue.Commands;
using FizzWare.NBuilder;
using Ironhide.Users.Domain.Application.CommandHandlers;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Users.Domain.Specs.CommandHandlers
{
    public class when_creating_google_user
    {
        static CreateGoogleLoginUser _command;
        static IUserRepository _userRepo;
        static IEventedCommandHandler<IUserSession, CreateGoogleLoginUser> _handler;
        static UserGoogleCreated _expectedEvent;
        static object _eventRaised;
        static UserGoogleLogin _userCreated;

        Establish context =
            () =>
            {
                _command = Builder<CreateGoogleLoginUser>.CreateNew().Build();

                _userRepo = Mock.Of<IUserRepository>();

                _userCreated = Builder<UserGoogleLogin>.CreateNew()
                    .With(user => user.Email, _command.Email)
                    .With(user => user.Name, _command.DisplayName)
                    .With(user => user.GoogleId, _command.Id)
                    .With(user => user.FirstName, _command.GivenName)
                    .With(user => user.ImageUrl, _command.ImageUrl)
                    .With(user => user.LastName, _command.FamilyName)
                    .With(user => user.URL, _command.Url)
                    .With(user => user.Id, Guid.NewGuid())
                    .Build();

                _handler = new UserGoogleCreator(_userRepo);

                Mock.Get(_userRepo)
                    .Setup(repository => repository.Create(Moq.It.IsAny<UserGoogleLogin>()))
                    .ReturnsAsync(_userCreated);

                _expectedEvent = new UserGoogleCreated(_userCreated.Id, _command.Email, _command.DisplayName,
                    _command.Id);
                _handler.NotifyObservers += x => _eventRaised = x;
            };

        Because of =
            () => _handler.Handle(Mock.Of<IUserSession>(), _command);

        It should_create_google_user =
            () => Mock.Get(_userRepo).Verify(
                repository =>
                    repository.Create(Moq.It.Is<UserGoogleLogin>(
                        z => z.FirstName == _command.GivenName &&
                             z.LastName == _command.FamilyName &&
                             z.GoogleId == _command.Id &&
                             z.ImageUrl == _command.ImageUrl &&
                             z.URL == _command.Url &&
                             z.Name == _command.DisplayName)
                        ));

        It should_throw_the_expected_event =
            () => _eventRaised.ShouldBeLike(_expectedEvent);
    }
}