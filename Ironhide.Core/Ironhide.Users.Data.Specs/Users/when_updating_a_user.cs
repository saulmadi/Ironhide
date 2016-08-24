using System;
using System.Linq;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Services;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Users
{
    public class when_updating_a_user
    {
        static ISampleRepository _repo;
        static readonly Guid UserId = Guid.NewGuid();
        static TestAppDataContext _dataContext;
        static User _userToUpdate;

        Establish context =
            () =>
            {
                _dataContext = new TestAppDataContext();
                _userToUpdate = new User(UserId, "name", "email");
                _dataContext.Seed(_userToUpdate);

                _repo = new SampleRepository(_dataContext);

                _userToUpdate.ChangeName("new name");
            };

        Because of =
            () => _repo.Update(_userToUpdate).Await();

        It should_change_the_values_in_the_database =
            () => _dataContext.Users.First(x => x.Id == UserId).Name.ShouldEqual("new name");
    }
}