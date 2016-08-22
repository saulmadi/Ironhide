using System;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Data.Specs.Support;
using Ironhide.Users.Domain;
using Machine.Specifications;

namespace Ironhide.Users.Data.Specs.Users
{
    public class when_finding_one_user_match_by_any_property
    {
        const string Name = "Mickey Mouse";
        static UserRepository _repo;
        static User _result;
        static TestUserDataContext _dataContext;
        static User _exactUserByReference;

        Establish context =
            () =>
            {
                _exactUserByReference = new User(Guid.NewGuid(), Name, "email");
                _dataContext = new TestUserDataContext();
                _dataContext.Seed(_exactUserByReference);

                _repo = new UserRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.First(x => x.Name == Name).Await();

        It should_return_the_matching_user_based_on_the_query =
            () => _result.ShouldEqual(_exactUserByReference);
    }
}