using System;
using System.Collections.Generic;
using AcklenAvenue.Commands;
using Ironhide.Common;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Exceptions;
using Ironhide.Users.Domain.Validators;
using Ironhide.Users.Domain.ValueObjects;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Users.Domain.Specs.Validation
{
    public class when_validating_a_password_reset_command_with_non_existent_reset_token
    {
        static ICommandValidator<IUserSession, ResetPassword> _validator;
        static readonly EncryptedPassword EncryptedPassword = new EncryptedPassword("password");
        static readonly Guid ResetPasswordToken = Guid.NewGuid();
        static List<ValidationFailure> _expectedFailures;
        static Exception _exception;
        static IPasswordResetTokenRepository _readOnlyRepo;

        Establish context =
            () =>
            {
                _readOnlyRepo = Mock.Of<IPasswordResetTokenRepository>();
                _validator = new SampleValidator(_readOnlyRepo, Mock.Of<ITimeProvider>());

                _expectedFailures = new List<ValidationFailure>
                                    {
                                        new ValidationFailure(
                                            "ResetPasswordToken",
                                            ValidationFailureType.DoesNotExist)
                                    };

                Mock.Get(_readOnlyRepo).Setup(x => x.GetById(ResetPasswordToken))
                    .Throws(new ItemNotFoundException<Sample>(ResetPasswordToken));
            };

        Because of =
            () => _exception = Catch.Exception(() =>
                _validator.Validate(new VisitorSession(),
                    new ResetPassword(ResetPasswordToken, EncryptedPassword)).Await());

        It should_return_expected_failures =
            () => ((CommandValidationException) _exception).ValidationFailures.ShouldBeLike(_expectedFailures);
    }
}