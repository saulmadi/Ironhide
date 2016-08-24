using System;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Ironhide.Users.Domain.Entities;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Roles
{
    public class when_finding_one_role_match_by_any_property
    {
        static RoleRepository _repo;
        static Role _result;
        static TestAppDataContext _dataContext;
        static Role _exactRoleByReference;

        Establish context =
            () =>
            {
                _exactRoleByReference = new Role(Guid.NewGuid(), "King");
                _dataContext = new TestAppDataContext();
                _dataContext.Seed(_exactRoleByReference);
                _repo = new RoleRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.First(x => x.Description == "King").Await();

        It should_return_the_matching_role_based_on_the_query =
            () => _result.ShouldEqual(_exactRoleByReference);
    }
}