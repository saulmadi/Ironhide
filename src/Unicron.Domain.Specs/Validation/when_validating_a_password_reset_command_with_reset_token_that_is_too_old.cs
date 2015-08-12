using System;
using System.Collections.Generic;
using AcklenAvenue.Commands;
using Machine.Specifications;
using Moq;
using Unicron.Users.Domain;
using Unicron.Users.Domain.Application.Commands;
using Unicron.Users.Domain.Entities;
using Unicron.Users.Domain.Exceptions;
using Unicron.Users.Domain.Services;
using Unicron.Users.Domain.Validators;
using Unicron.Users.Domain.ValueObjects;
using It = Machine.Specifications.It;

namespace Unicron.Domain.Specs.Validation
{
    public class when_validating_a_password_reset_command_with_reset_token_that_is_too_old
    {
        static ICommandValidator<ResetPassword> _validator;
        static readonly EncryptedPassword EncryptedPassword = new EncryptedPassword("password");
        static readonly Guid ResetPasswordToken = Guid.NewGuid();
        static List<ValidationFailure> _expectedFailures;
        static Exception _exception;
        static IReadOnlyRepository _readOnlyRepo;
        static DateTime _now;
        static ITimeProvider _timeProvider;

        Establish context =
            () =>
            {
                _readOnlyRepo = Mock.Of<IReadOnlyRepository>();
                _timeProvider = Mock.Of<ITimeProvider>();
                _validator = new PassowrdResetValidator(_readOnlyRepo, _timeProvider);

                _expectedFailures = new List<ValidationFailure>
                                    {
                                        new ValidationFailure(
                                            "ResetPasswordToken",
                                            ValidationFailureType.Expired)
                                    };

                _now = DateTime.Now;
                Mock.Get(_timeProvider).Setup(x => x.Now()).Returns(_now);

                Mock.Get(_readOnlyRepo).Setup(x => x.GetById<PasswordResetAuthorization>(ResetPasswordToken))
                    .Returns(new PasswordResetAuthorization(ResetPasswordToken, Guid.NewGuid(), _now.AddDays(3)));
            };

        Because of =
            () => _exception = Catch.Exception(() =>
                _validator.Validate(new VisitorSession(),
                    new ResetPassword(ResetPasswordToken, EncryptedPassword)));

        It should_return_expected_failures =
            () => ((CommandValidationException) _exception).ValidationFailures.ShouldBeLike(_expectedFailures);
    }
}