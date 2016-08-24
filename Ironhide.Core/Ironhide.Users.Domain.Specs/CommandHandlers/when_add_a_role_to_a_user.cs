using System;
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
        static ISampleRepository _sampleReadRepo;
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

                _sampleReadRepo = Mock.Of<ISampleRepository>();
                _roleReadRepo = Mock.Of<IRoleRepository>();

                Mock.Get(_sampleReadRepo)
                    .Setup(repo => repo.GetById<User>(_userCreated.Id)).ReturnsAsync(_userCreated);

                Mock.Get(_roleReadRepo)
                    .Setup(repository => repository.GetById(_rolAdded.Id)).ReturnsAsync(_rolAdded);

                _handler = new UserRoleAdder(_sampleReadRepo, _roleReadRepo);

                _expectedEvent = new UserRoleAdded(_userCreated.Id, _rolAdded.Id);
                _handler.NotifyObservers += x => _eventRaised = x;
            };

        Because of =
            () => _handler.Handle(Mock.Of<IUserSession>(), _command);

        It should_throw_the_expected_event =
            () => _eventRaised.ShouldBeLike(_expectedEvent);

        It should_update_the_users_roles_in_the_database =
            () => Mock.Get(_sampleReadRepo)
                .Verify(
                    repository =>
                        repository.Update(
                            Moq.It.Is<User>(user => user.Id == _userCreated.Id && user.UserRoles.Contains(_rolAdded))));
    }
}