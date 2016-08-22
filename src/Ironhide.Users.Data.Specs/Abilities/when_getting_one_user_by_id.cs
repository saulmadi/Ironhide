using System;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Data.Specs.Support;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;
using Machine.Specifications;

namespace Ironhide.Users.Data.Specs.Abilities
{
    public class when_getting_one_user_ability_by_id
    {
        static IUserAbilityRepository _repo;
        static UserAbility _result;
        static TestUserDataContext _dataContext;
        static UserAbility _exactAbilityByReference;
        static Guid _entityId;

        Establish context =
            () =>
            {
                _entityId = Guid.NewGuid();
                _exactAbilityByReference = new UserAbility(_entityId, "Writing");
                _dataContext = new TestUserDataContext();
                _dataContext.Seed(_exactAbilityByReference);
                _repo = new UserAbilityRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.GetById(_entityId).Await();

        It should_return_the_matching_ability_by_id =
            () => _result.ShouldEqual(_exactAbilityByReference);
    }
}