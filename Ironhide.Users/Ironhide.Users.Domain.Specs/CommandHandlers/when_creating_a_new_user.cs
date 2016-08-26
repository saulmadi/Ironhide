﻿using System;
using System.Collections.Generic;
using System.Linq;
using AcklenAvenue.Commands;
using FizzWare.NBuilder;
using Ironhide.Users.Domain.Application.CommandHandlers;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Ironhide.Users.Domain.ValueObjects;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Users.Domain.Specs.CommandHandlers
{
    public class when_creating_a_new_user
    {
        static CreateEmailLoginUser _command;
        static IUserRepository _userRepo;
        static IUserAbilityRepository _abilityReadRepo;
        static IEventedCommandHandler<IUserSession, CreateEmailLoginUser> _handler;
        static UserEmailCreated _expectedEvent;
        static object _eventRaised;
        static UserEmailLogin _userCreated;
        static IEnumerable<UserAbility> _userAbilities;
        static UserAbility _userAbility;
        static IRoleRepository _roleReadRepo;


        Establish context =
            () =>
            {
                _userAbility = Builder<UserAbility>.CreateNew()
                    .Build();
                _userAbilities = new List<UserAbility> {_userAbility};
                _command = new CreateEmailLoginUser("email", new EncryptedPassword("password"), "name", "password",
                    _userAbilities);

                _userCreated = Builder<UserEmailLogin>.CreateNew()
                    .With(user => user.Id, Guid.NewGuid())
                    .With(user => user.Email, _command.Email)
                    .With(user => user.Name, _command.Name)
                    .With(user => user.EncryptedPassword, _command.EncryptedPassword.Password)
                    .With(user => user.PhoneNumber, _command.PhoneNumber)
                    .Build();
                _userCreated.AddAbility(_userAbility);

                _userRepo = Mock.Of<IUserRepository>();
                _abilityReadRepo = Mock.Of<IUserAbilityRepository>();
                _roleReadRepo = Mock.Of<IRoleRepository>();

                Mock.Get(_abilityReadRepo)
                    .Setup(repository => repository.GetById(_userAbility.Id)).ReturnsAsync(_userAbility);
                Mock.Get(_userRepo)
                    .Setup(repository => repository.Create(Moq.It.IsAny<UserEmailLogin>()))
                    .ReturnsAsync(_userCreated);

                _handler = new UserEmailCreator(_roleReadRepo, _abilityReadRepo, _userRepo);

                _expectedEvent = new UserEmailCreated(_userCreated.Id, _command.Email, _command.Name,
                    _command.PhoneNumber);
                _handler.NotifyObservers += x => _eventRaised = x;
            };

        Because of =
            () => _handler.Handle(Mock.Of<IUserSession>(), _command);

        It should_create_the_new_user =
            () => Mock.Get(_userRepo).Verify(
                x =>
                    x.Create(Moq.It.Is<UserEmailLogin>(u =>
                        u.Name == _command.Name
                        && u.Email == _command.Email
                        && u.EncryptedPassword == _command.EncryptedPassword.Password
                        && u.PhoneNumber == _command.PhoneNumber
                        && u.UserAbilities.Contains(_userAbility)
                        )));


        It should_throw_the_expected_event =
            () => _eventRaised.ShouldBeLike(_expectedEvent);
    }
}