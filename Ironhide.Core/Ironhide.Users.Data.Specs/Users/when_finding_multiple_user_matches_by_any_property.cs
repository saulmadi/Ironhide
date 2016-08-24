using System;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Ironhide.Users.Domain;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Users
{
    public class when_finding_multiple_user_matches_by_any_property
    {
        static SampleRepository _repo;
        static IEnumerable<User> _result;
        static TestAppDataContext _dataContext;
        static IList<User> _users;

        Establish context =
            () =>
            {
                _users = Builder<User>.CreateListOfSize(5)
                    .TheLast(3)
                    .With(x => x.Name, "Mickey " + new Random().Next())
                    .Build();

                _dataContext = new TestAppDataContext();
                _dataContext.Seed(_users.ToArray());

                _repo = new SampleRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.Query<User>(x => x.Name.StartsWith("Mickey")).WaitForResult();

        It should_return_the_users_that_match_the_query =
            () => _result.ShouldContainOnly(_users[2], _users[3], _users[4]);
    }
}