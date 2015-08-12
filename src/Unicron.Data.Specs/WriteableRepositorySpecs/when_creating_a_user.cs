using System;
using AcklenAvenue.Data.NHibernate.Testing;
using Machine.Specifications;
using NHibernate;
using Unicron.Users.Domain.Entities;
using Unicron.Users.Domain.Services;
using Unicron.Users.Domain.ValueObjects;

namespace Unicron.Data.Specs.WriteableRepositorySpecs
{
    public class when_creating_a_user
    {
        static IWriteableRepository _writeableRepository;
        static UserEmailLogin _result;
        static ISession _session;

        Establish context =
            () =>
                {
                    _session = InMemorySession.New(new MappingScheme("Test"));
                    _writeableRepository = new WriteableRepository(_session);
                };

        Because of =
            () =>
            _result =
            _writeableRepository.Create(new UserEmailLogin("test", "test@test.com", new EncryptedPassword("password")));

        It should_be_retrievable =
            () => _session.Get<UserEmailLogin>(_result.Id).Name.ShouldEqual("test");

        It should_return_the_created_user_with_an_id =
            () => _result.Id.ShouldNotEqual(Guid.Empty);
    }
}