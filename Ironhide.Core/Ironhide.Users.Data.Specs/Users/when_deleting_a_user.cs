using System;
using System.Linq;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Services;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Users
{
    public class when_deleting_a_user
    {
        static ISampleRepository _repo;
        static readonly Guid UserId = Guid.NewGuid();
        static TestAppDataContext _dataContext;

        Establish context =
            () =>
            {
                _dataContext = new TestAppDataContext();
                _dataContext.Seed(new User(UserId, "name", "email"));
                _dataContext.Seed(new User(Guid.NewGuid(), "another name", "email"));

                _repo = new SampleRepository(_dataContext);
            };

        Because of =
            () => _repo.Delete(UserId).Await();

        It should_leave_other_users_alone =
            () => _dataContext.Users.First(x => x.Name == "another name").ShouldNotBeNull();

        It should_remove_the_user_from_the_database =
            () => _dataContext.Users.Where(x => x.Id == UserId).ShouldBeEmpty();
    }
}