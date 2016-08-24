using System;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Exceptions;
using Ironhide.Users.Domain.Services;
using Ironhide.Users.Domain.Validators;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Users.Domain.Specs.Validation
{
    public class when_validating_a_password_reset_request_where_email_is_missing
    {
        static ICommandValidator<IUserSession, StartPasswordResetProcess> _validator;
        static Exception _exception;
        static IUserRepository _readOnlyRepsitory;

        Establish context =
            () =>
            {
                _readOnlyRepsitory = Mock.Of<IUserRepository>();
                _validator = new StartPasswordResetProcessValidator(_readOnlyRepsitory);
            };

        Because of =
            () =>
                _exception = Catch.Exception(() => _validator.Validate(null, new StartPasswordResetProcess("")).Await());

        It should_return_a_validation_failure_for_missing_email_address =
            () =>
                _exception.As<CommandValidationException>().ValidationFailures
                    .ShouldContain(x => x.Property == "Email" &&
                                        x.FailureType == ValidationFailureType.Missing);
    }
}