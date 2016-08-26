using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Data.Specs.Support;
using Ironhide.Users.Domain.Entities;
using Machine.Specifications;

namespace Ironhide.Users.Data.Specs.Abilities
{
    public class when_getting_all_user_abilities
    {
        static UserAbilityRepository _repo;
        static IEnumerable<UserAbility> _result;
        static TestUserDataContext _dataContext;
        static IList<UserAbility> _allUserAbilities;

        Establish context =
            () =>
            {
                _allUserAbilities = Builder<UserAbility>.CreateListOfSize(5).Build();
                _dataContext = new TestUserDataContext();
                _dataContext.Seed(_allUserAbilities.ToArray());
                _repo = new UserAbilityRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.GetAll().WaitForResult();

        It should_return_all_the_user_abilities =
            () => _result.ShouldContain(_allUserAbilities);
    }
}