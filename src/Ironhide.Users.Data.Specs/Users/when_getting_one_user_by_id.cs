using System;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Data.Specs.Support;
using Ironhide.Users.Domain;
using Machine.Specifications;

namespace Ironhide.Users.Data.Specs.Users
{
    public class when_getting_one_user_by_id
    {
        static UserRepository _repo;
        static User _result;
        static TestUserDataContext _dataContext;
        static User _exactUserByReference;
        static Guid _userId;

        Establish context =
            () =>
            {
                _userId = Guid.NewGuid();
                _exactUserByReference = new User(_userId, "name", "email");
                _dataContext = new TestUserDataContext();
                _dataContext.Seed(_exactUserByReference);
                _repo = new UserRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.GetById(_userId).Await();

        It should_return_the_matching_user_by_id =
            () => _result.ShouldEqual(_exactUserByReference);
    }
}