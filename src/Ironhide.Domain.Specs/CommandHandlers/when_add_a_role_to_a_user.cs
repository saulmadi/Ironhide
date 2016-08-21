﻿using System;
using System.Linq;
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
    public class when_add_a_role_to_a_user
    {
        static AddRoleToUser _command;
        static IWriteableRepository _writeableRepository;
        static IUserRepository<User> _userReadRepo;
        static IEventedCommandHandler<IUserSession, AddRoleToUser> _handler;
        static UserRoleAdded _expectedEvent;
        static object _eventRaised;
        static User _userCreated;
        static IIdentityGenerator<Guid> _identityGenerator;
        static Role _rolAdded;
        static IRoleRepository _roleReadRepo;

        Establish context =
            () =>
            {
                _identityGenerator = Mock.Of<IIdentityGenerator<Guid>>();
                Mock.Get(_identityGenerator).Setup(generator => generator.Generate()).Returns(Guid.NewGuid);

                _command = Builder<AddRoleToUser>.CreateNew().Build();
                _userCreated = Builder<User>.CreateNew().Build();
                _rolAdded = Builder<Role>.CreateNew().Build();

                _writeableRepository = Mock.Of<IWriteableRepository>();
                _userReadRepo = Mock.Of<IUserRepository<User>>();
                _roleReadRepo = Mock.Of<IRoleRepository>();

                Mock.Get(_writeableRepository)
                    .Setup(repository => repository.Update(Moq.It.Is<User>(user => user.Id == _userCreated.Id)))
                    .ReturnsAsync(_userCreated);

                Mock.Get(_userReadRepo)
                    .Setup(repo => repo.GetById(_userCreated.Id)).ReturnsAsync(_userCreated);

                Mock.Get(_roleReadRepo)
                    .Setup(repository => repository.GetById(_rolAdded.Id)).ReturnsAsync(_rolAdded);

                _handler = new UserRoleAdder(_writeableRepository, _userReadRepo, _roleReadRepo);

                _expectedEvent = new UserRoleAdded(_userCreated.Id, _rolAdded.Id);
                _handler.NotifyObservers += x => _eventRaised = x;
            };

        Because of =
            () => _handler.Handle(Mock.Of<IUserSession>(), _command);

        It should_create_the_new_user =
            () => Mock.Get(_writeableRepository).Verify(
                x =>
                    x.Update(Moq.It.Is<User>(u =>
                        u.Id == _userCreated.Id && u.UserRoles.Contains(_rolAdded))));

        It should_throw_the_expected_event =
            () => _eventRaised.ShouldBeLike(_expectedEvent);
    }
}