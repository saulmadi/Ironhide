using System;
using System.Collections.Generic;
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
    public class when_something_happens
    {
        static IIdentityGenerator<Guid> _identityGenerator;
        static Guid _userGuid;
        static AddAbilitiesToUser _addAbilitiesToUser;
        static UserAbility _abilities;
        static Guid _abilityGuid;
        static IWriteableRepository _writeableRepository;
        static IUserRepository<User> _userReadRepo;
        static User _userCreated;
        static UserAbilitiesAdder _handle;
        static UserAbilitiesAdded _userAbilitiesAdded;
        static object _eventRaised;
        static List<UserAbility> _abilitiesAdded;
        static IUserAbilityRepository _abilityReadRepo;

        Establish context =
            () =>
            {
                _identityGenerator = Mock.Of<IIdentityGenerator<Guid>>();
                Mock.Get(_identityGenerator).Setup(generator => generator.Generate()).Returns(Guid.NewGuid);
                _userGuid = _identityGenerator.Generate();
                _abilityGuid = _identityGenerator.Generate();

                _abilities = Builder<UserAbility>.CreateNew()
                    .With(x => x.Id, _abilityGuid)
                    .With(x => x.Description, "Developer")
                    .Build();

                _userCreated = Builder<User>.CreateNew()
                    .With(user => user.Email, "a@a.com")
                    .With(user => user.Name, "user")
                    .With(user => user.Id, _userGuid)
                    .Build();

                _abilitiesAdded = new List<UserAbility> {_abilities};
                _addAbilitiesToUser = new AddAbilitiesToUser(_userGuid, _abilitiesAdded.Select(x => x.Id));

                _writeableRepository = Mock.Of<IWriteableRepository>();
                _userReadRepo = Mock.Of<IUserRepository<User>>();
                _abilityReadRepo = Mock.Of<IUserAbilityRepository>();

                Mock.Get(_writeableRepository)
                    .Setup(repository => repository.Update(Moq.It.Is<User>(user => user.Id == _userGuid)))
                    .ReturnsAsync(_userCreated);

                Mock.Get(_userReadRepo)
                    .Setup(repository => repository.GetById(_userCreated.Id)).ReturnsAsync(_userCreated);

                Mock.Get(_abilityReadRepo)
                    .Setup(repository => repository.GetById(_abilityGuid))
                    .ReturnsAsync(_abilitiesAdded.FirstOrDefault());

                _handle = new UserAbilitiesAdder(_writeableRepository, _userReadRepo, _abilityReadRepo);
                _userAbilitiesAdded = new UserAbilitiesAdded(_userGuid, _abilitiesAdded.Select(x => x.Id));
                _handle.NotifyObservers += x => _eventRaised = x;
            };

        Because of =
            () => _handle.Handle(Mock.Of<IUserSession>(), _addAbilitiesToUser);

        It should_add_abilities_to_user =
            () =>
                Mock.Get(_writeableRepository).Verify(
                    x =>
                        x.Update(Moq.It.Is<User>(u =>
                            u.Id == _userCreated.Id && u.UserAbilities.Contains(_abilities))));

        It should_return_expected_event =
            () =>
                _eventRaised.ShouldBeLike(_userAbilitiesAdded);
    }
}