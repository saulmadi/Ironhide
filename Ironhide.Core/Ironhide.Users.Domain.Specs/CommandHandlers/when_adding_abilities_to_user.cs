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
    public class when_adding_abilitites_to_an_existing_user
    {
        static IIdentityGenerator<Guid> _identityGenerator;
        static Guid _userGuid;
        static AddSample _addAbilitiesToUser;
        static UserAbility _abilities;
        static Guid _abilityGuid;
        static ISampleRepository _sampleRepo;
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
                _addAbilitiesToUser = new AddSample(_userGuid, _abilitiesAdded.Select(x => x.Id));

                _sampleRepo = Mock.Of<ISampleRepository>();
                _abilityReadRepo = Mock.Of<IUserAbilityRepository>();

                Mock.Get(_sampleRepo)
                    .Setup(repository => repository.GetById<User>(_userCreated.Id)).ReturnsAsync(_userCreated);

                Mock.Get(_abilityReadRepo)
                    .Setup(repository => repository.GetById(_abilityGuid))
                    .ReturnsAsync(_abilitiesAdded.FirstOrDefault());

                _handle = new UserAbilitiesAdder(_sampleRepo, _abilityReadRepo);
                _userAbilitiesAdded = new UserAbilitiesAdded(_userGuid, _abilitiesAdded.Select(x => x.Id));
                _handle.NotifyObservers += x => _eventRaised = x;
            };

        Because of =
            () => _handle.Handle(Mock.Of<IUserSession>(), _addAbilitiesToUser);

        It should_add_abilities_to_user =
            () =>
                Mock.Get(_sampleRepo).Verify(
                    x =>
                        x.Update(Moq.It.Is<User>(u =>
                            u.Id == _userCreated.Id && u.UserAbilities.Contains(_abilities))));

        It should_return_expected_event =
            () =>
                _eventRaised.ShouldBeLike(_userAbilitiesAdded);
    }
}