using System;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Ironhide.Users.Domain;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Users
{
    public class when_getting_one_user_by_id
    {
        static SampleRepository _repo;
        static User _result;
        static TestAppDataContext _dataContext;
        static User _exactUserByReference;
        static Guid _userId;

        Establish context =
            () =>
            {
                _userId = Guid.NewGuid();
                _exactUserByReference = new User(_userId, "name", "email");
                _dataContext = new TestAppDataContext();
                _dataContext.Seed(_exactUserByReference);
                _repo = new SampleRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.GetById<User>(_userId).Await();

        It should_return_the_matching_user_by_id =
            () => _result.ShouldEqual(_exactUserByReference);
    }
}