using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Data.Specs.Support;
using Ironhide.Users.Domain.Entities;
using Machine.Specifications;

namespace Ironhide.Users.Data.Specs.Users
{
    public class when_getting_all_users
    {
        static UserRepository _repo;
        static IEnumerable<User> _result;
        static TestUserDataContext _dataContext;
        static IList<User> _users;

        Establish context =
            () =>
            {
                _users = Builder<User>.CreateListOfSize(5).Build();
                _dataContext = new TestUserDataContext();
                _dataContext.Seed(_users.ToArray());
                _repo = new UserRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.GetAll().WaitForResult();

        It should_return_all_the_users =
            () => _result.ShouldContain(_users);
    }
}