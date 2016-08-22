using System;
using System.Linq;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Data.Specs.Support;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Services;
using Machine.Specifications;

namespace Ironhide.Users.Data.Specs.Users
{
    public class when_creating_a_user
    {
        static IUserRepository _repo;
        static readonly Guid UserId = Guid.NewGuid();
        static TestUserDataContext _dataContext;

        Establish context =
            () =>
            {
                _dataContext = new TestUserDataContext();
                _repo = new UserRepository(_dataContext);
            };

        Because of =
            () => _repo.Create(new User(UserId, "name", "email")).Await();

        It should_be_discoverable =
            () => _dataContext.Users.Where(x => x.Id == UserId).ShouldNotBeEmpty();
    }
}