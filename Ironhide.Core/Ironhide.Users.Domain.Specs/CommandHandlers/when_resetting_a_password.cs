using System;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.CommandHandlers;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;
using Ironhide.Users.Domain.ValueObjects;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Users.Domain.Specs.CommandHandlers
{
    public class when_resetting_a_password
    {
        const string NewPassword = "new_password";
        static ISampleRepository _sampleRepo;
        static IEventedCommandHandler<IUserSession, ResetPassword> _commandHander;
        static readonly Guid ResetPasswordToken = Guid.NewGuid();
        static object _eventRaised;
        static SampleUpdated _expectedEvent;
        static IPasswordResetTokenRepository _tokenReadRepo;

        Establish context =
            () =>
            {
                _sampleRepo = Mock.Of<ISampleRepository>();
                _tokenReadRepo = Mock.Of<IPasswordResetTokenRepository>();
                _commandHander =
                    new SampleAdder(_sampleRepo, _tokenReadRepo);

                Guid userId = Guid.NewGuid();
                Mock.Get(_tokenReadRepo).Setup(x => x.GetById(ResetPasswordToken))
                    .ReturnsAsync(new Sample(ResetPasswordToken, userId, DateTime.Now));

                Mock.Get(_sampleRepo).Setup(x => x.GetById<UserEmailLogin>(userId))
                    .ReturnsAsync(new UserEmailLogin(userId, "name", "password", new EncryptedPassword("123")));

                _commandHander.NotifyObservers += x => _eventRaised = x;
                _expectedEvent = new SampleUpdated(userId);
            };

        Because of =
            () =>
                _commandHander.Handle(new VisitorSession(),
                    new ResetPassword(ResetPasswordToken, new EncryptedPassword(NewPassword)));

        It should_change_the_password_in_the_user =
            () =>
                Mock.Get(_sampleRepo)
                    .Verify(x => x.Update(Moq.It.Is<UserEmailLogin>(y => y.EncryptedPassword == NewPassword)));

        It should_notify_observers =
            () => _eventRaised.ShouldBeLike(_expectedEvent);

        It should_remove_the_token =
            () => Mock.Get(_tokenReadRepo).Verify(x => x.Delete(ResetPasswordToken));
    }
}