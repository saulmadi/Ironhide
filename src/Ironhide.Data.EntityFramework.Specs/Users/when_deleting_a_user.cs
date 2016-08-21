using System;
using System.Linq;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Data.Specs.Support;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Machine.Specifications;

namespace Ironhide.Users.Data.Specs.Users
{
    public class when_deleting_a_user
    {
        static IUserRepository<User> _repo;
        static readonly Guid UserId = Guid.NewGuid();
        static TestUserDataContext _dataContext;

        Establish context =
            () =>
            {
                _dataContext = new TestUserDataContext();
                _dataContext.Seed(new User(UserId, "name", "email"));
                _dataContext.Seed(new User(Guid.NewGuid(), "another name", "email"));

                _repo = new UserRepository(_dataContext);
            };

        Because of =
            () => _repo.Delete(UserId).Await();

        It should_leave_other_users_alone =
            () => _dataContext.Users.First(x => x.Name == "another name").ShouldNotBeNull();

        It should_remove_the_user_from_the_database =
            () => _dataContext.Users.Where(x => x.Id == UserId).ShouldBeEmpty();
    }
}