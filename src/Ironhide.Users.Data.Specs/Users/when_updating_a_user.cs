using System;
using System.Linq;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Data.Specs.Support;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Services;
using Machine.Specifications;

namespace Ironhide.Users.Data.Specs.Users
{
    public class when_updating_a_user
    {
        static IUserRepository<User> _repo;
        static readonly Guid UserId = Guid.NewGuid();
        static TestUserDataContext _dataContext;
        static User _userToUpdate;

        Establish context =
            () =>
            {
                _dataContext = new TestUserDataContext();
                _userToUpdate = new User(UserId, "name", "email");
                _dataContext.Seed(_userToUpdate);

                _repo = new UserRepository(_dataContext);

                _userToUpdate.ChangeName("new name");
            };

        Because of =
            () => _repo.Update(_userToUpdate).Await();

        It should_change_the_values_in_the_database =
            () => _dataContext.Users.First(x => x.Id == UserId).Name.ShouldEqual("new name");
    }
}