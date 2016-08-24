using System;
using Ironhide.App.Data.Repositories;
using Ironhide.App.Data.Specs.Support;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;
using Machine.Specifications;

namespace Ironhide.App.Data.Specs.Tokens
{
    public class when_getting_one_token_by_id
    {
        static IPasswordResetTokenRepository _repo;
        static Sample _result;
        static TestAppDataContext _dataContext;
        static Sample _exactTokenByReference;
        static Guid _entityId;

        Establish context =
            () =>
            {
                _entityId = Guid.NewGuid();
                _exactTokenByReference = new Sample(_entityId, Guid.NewGuid(), DateTime.Now);
                _dataContext = new TestAppDataContext();
                _dataContext.Seed(_exactTokenByReference);
                _repo = new PasswordResetTokenRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.GetById(_entityId).Await();

        It should_return_the_matching_role_by_id =
            () => _result.ShouldEqual(_exactTokenByReference);
    }
}