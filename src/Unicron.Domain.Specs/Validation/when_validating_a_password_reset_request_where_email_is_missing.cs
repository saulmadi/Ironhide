using System;
using AcklenAvenue.Commands;
using Machine.Specifications;
using Moq;
using Unicron.Users.Domain;
using Unicron.Users.Domain.Application.Commands;
using Unicron.Users.Domain.Exceptions;
using Unicron.Users.Domain.Services;
using Unicron.Users.Domain.Validators;
using It = Machine.Specifications.It;

namespace Unicron.Domain.Specs.Validation
{
    public class when_validating_a_password_reset_request_where_email_is_missing
    {
        static ICommandValidator<CreatePasswordResetToken> _validator;
        static Exception _exception;

        Establish context =
            () =>
            {
                _readOnlyRepsitory = Mock.Of<IReadOnlyRepository>();
                _validator = new PasswordResetValidator(_readOnlyRepsitory);
            };

        Because of =
            () => _exception = Catch.Exception(() => _validator.Validate(null, new CreatePasswordResetToken("")));

        It should_return_a_validation_failure_for_missing_email_address =
            () =>
                _exception.As<CommandValidationException>().ValidationFailures
                    .ShouldContain(x => x.Property == "Email" &&
                                        x.FailureType == ValidationFailureType.Missing);

        static IReadOnlyRepository _readOnlyRepsitory;
    }
}