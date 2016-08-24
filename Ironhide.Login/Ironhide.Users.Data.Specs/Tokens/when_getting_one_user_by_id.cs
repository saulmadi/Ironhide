using System;
using Ironhide.Users.Data.Repositories;
using Ironhide.Users.Data.Specs.Support;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;
using Machine.Specifications;

namespace Ironhide.Users.Data.Specs.Tokens
{
    public class when_getting_one_token_by_id
    {
        static IPasswordResetTokenRepository _repo;
        static PasswordResetToken _result;
        static TestUserDataContext _dataContext;
        static PasswordResetToken _exactTokenByReference;
        static Guid _entityId;

        Establish context =
            () =>
            {
                _entityId = Guid.NewGuid();
                _exactTokenByReference = new PasswordResetToken(_entityId, Guid.NewGuid(), DateTime.Now);
                _dataContext = new TestUserDataContext();
                _dataContext.Seed(_exactTokenByReference);
                _repo = new PasswordResetTokenRepository(_dataContext);
            };

        Because of =
            () => _result = _repo.GetById(_entityId).Await();

        It should_return_the_matching_role_by_id =
            () => _result.ShouldEqual(_exactTokenByReference);
    }
}