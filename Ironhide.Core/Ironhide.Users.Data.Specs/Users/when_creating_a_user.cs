using System;
using System.Linq;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Services;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Users
{
    public class when_creating_a_user
    {
        static ISampleRepository _repo;
        static readonly Guid UserId = Guid.NewGuid();
        static TestAppDataContext _dataContext;

        Establish context =
            () =>
            {
                _dataContext = new TestAppDataContext();
                _repo = new SampleRepository(_dataContext);
            };

        Because of =
            () => _repo.Create(new User(UserId, "name", "email")).Await();

        It should_be_discoverable =
            () => _dataContext.Users.Where(x => x.Id == UserId).ShouldNotBeEmpty();
    }
}