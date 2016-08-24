using System;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Ironhide.Users.Domain;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Users
{
    public class when_finding_one_user_match_by_any_property
    {
        const string Name = "Mickey Mouse";
        static SampleRepository _repo;
        static User _result;
        static TestAppDataContext _dataContext;
        static User _exactUserByReference;

        Establish context =
            () =>
            {
                _exactUserByReference = new User(Guid.NewGuid(), Name, "email");
                _dataContext = new TestAppDataContext();
                _dataContext.Seed(_exactUserByReference);

                _repo = new SampleRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.First<User>(x => x.Name == Name).Await();

        It should_return_the_matching_user_based_on_the_query =
            () => _result.ShouldEqual(_exactUserByReference);
    }
}