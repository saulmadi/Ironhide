using System;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Data.Specs.Support;
using Ironhide.Users.Domain.Entities;
using Machine.Specifications;

namespace Ironhide.Users.Data.Specs.Roles
{
    public class when_finding_one_role_match_by_any_property
    {
        static RoleRepository _repo;
        static Role _result;
        static TestUserDataContext _dataContext;
        static Role _exactRoleByReference;

        Establish context =
            () =>
            {
                _exactRoleByReference = new Role(Guid.NewGuid(), "King");
                _dataContext = new TestUserDataContext();
                _dataContext.Seed(_exactRoleByReference);
                _repo = new RoleRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.First(x => x.Description == "King").Await();

        It should_return_the_matching_role_based_on_the_query =
            () => _result.ShouldEqual(_exactRoleByReference);
    }
}