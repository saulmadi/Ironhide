using System;
using AcklenAvenue.Testing.Moq.ExpectedObjects;
using Machine.Specifications;
using Moq;
using Unicron.Users.Domain;
using Unicron.Users.Domain.DomainEvents;
using Unicron.Users.Domain.Entities;
using Unicron.Users.Domain.Services;
using Unicron.Users.Domain.ValueObjects;
using Unicron.Web.Api;
using It = Machine.Specifications.It;

namespace Unicron.Web.Specs
{
    public class when_sending_a_an_email_after_creating_a_password_reset_token
    {
        const string Email = "bob@email.com";
        const string BaseUrl = "http://baseurl";
        static PasswordResetEmailSender _eventHandler;
        static readonly Guid UserId = Guid.NewGuid();
        static readonly Guid TokenId = Guid.NewGuid();
        static IEmailSender _emailSender;

        Establish context =
            () =>
            {
                var readOnlyRepository = Mock.Of<IReadOnlyRepository>();
                _emailSender = Mock.Of<IEmailSender>();
                var _baseUrlProvider = Mock.Of<IBaseUrlProvider>();
                _eventHandler = new PasswordResetEmailSender(readOnlyRepository, _emailSender, _baseUrlProvider);

                Mock.Get(_baseUrlProvider).Setup(x => x.GetBaseUrl()).Returns(BaseUrl);

                var value = new UserEmailLogin("Bob", Email, new EncryptedPassword("something"));
                Mock.Get(readOnlyRepository)
                    .Setup(x => x.GetById<UserEmailLogin>(UserId))
                    .Returns(value);
            };

        Because of =
            () => _eventHandler.Handle(new PasswordResetTokenCreated(TokenId, UserId));

        It should_send_the_email =
            () =>
                Mock.Get(_emailSender)
                    .Verify(x => x.Send(Email, WithExpected.Object(new PasswordResetEmail(BaseUrl, TokenId))));

    }
}