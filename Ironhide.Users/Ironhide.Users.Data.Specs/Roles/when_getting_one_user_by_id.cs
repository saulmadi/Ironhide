using System;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Data.Specs.Support;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;
using Machine.Specifications;

namespace Ironhide.Users.Data.Specs.Roles
{
    public class when_getting_one_role_by_id
    {
        static IRoleRepository _repo;
        static Role _result;
        static TestUserDataContext _dataContext;
        static Role _exactRoleByReference;
        static Guid _roleId;

        Establish context =
            () =>
            {
                _roleId = Guid.NewGuid();
                _exactRoleByReference = new Role(_roleId, "King");
                _dataContext = new TestUserDataContext();
                _dataContext.Seed(_exactRoleByReference);
                _repo = new RoleRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.GetById(_roleId).Await();

        It should_return_the_matching_role_by_id =
            () => _result.ShouldEqual(_exactRoleByReference);
    }
}