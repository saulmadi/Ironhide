using System;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.CommandHandlers;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Ironhide.Users.Domain.Specs.Stubs;
using Ironhide.Users.Domain.ValueObjects;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Users.Domain.Specs.CommandHandlers
{
    public class when_resetting_a_password
    {
        const string NewPassword = "new_password";
        static IWriteableRepository _writeableRepository;
        static IUserRepository<UserEmailLogin> _userReadRepo;
        static IEventedCommandHandler<IUserSession, ResetPassword> _commandHander;
        static readonly Guid ResetPasswordToken = Guid.NewGuid();
        static object _eventRaised;
        static PasswordReset _expectedEvent;
        static IPasswordResetTokenRepository _tokenReadRepo;

        Establish context =
            () =>
            {
                _writeableRepository = Mock.Of<IWriteableRepository>();
                _userReadRepo = Mock.Of<IUserRepository<UserEmailLogin>>();
                _tokenReadRepo = Mock.Of<IPasswordResetTokenRepository>();
                _commandHander =
                    new PasswordResetter(_userReadRepo, _tokenReadRepo, _writeableRepository);

                Guid userId = Guid.NewGuid();
                Mock.Get(_tokenReadRepo).Setup(x => x.GetById(ResetPasswordToken))
                    .ReturnsAsync(new PasswordResetToken(ResetPasswordToken, userId, DateTime.Now));

                Mock.Get(_userReadRepo).Setup(x => x.GetById(userId))
                    .ReturnsAsync(new TestUser(userId, "name", "password"));

                _commandHander.NotifyObservers += x => _eventRaised = x;
                _expectedEvent = new PasswordReset(userId);
            };

        Because of =
            () =>
                _commandHander.Handle(new VisitorSession(),
                    new ResetPassword(ResetPasswordToken, new EncryptedPassword(NewPassword)));

        It should_change_the_password_in_the_user =
            () =>
                Mock.Get(_writeableRepository)
                    .Verify(x => x.Update(Moq.It.Is<UserEmailLogin>(y => y.EncryptedPassword == NewPassword)));

        It should_notify_observers =
            () => _eventRaised.ShouldBeLike(_expectedEvent);

        It should_remove_the_token =
            () => Mock.Get(_writeableRepository).Verify(x => x.Delete<PasswordResetToken>(ResetPasswordToken));
    }
}