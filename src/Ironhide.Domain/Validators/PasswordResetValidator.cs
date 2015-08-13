using System.Collections.Generic;
using System.Linq;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Exceptions;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Validators
{
    public class PasswordResetValidator : ICommandValidator<CreatePasswordResetToken>
    {
        readonly IReadOnlyRepository _readOnlyRepsitory;

        public PasswordResetValidator(IReadOnlyRepository readOnlyRepsitory)
        {
            _readOnlyRepsitory = readOnlyRepsitory;
        }

        public void Validate(IUserSession userSession, CreatePasswordResetToken command)
        {
            var validationFailures = new List<ValidationFailure>();

            if (string.IsNullOrEmpty(command.Email))
                validationFailures.Add(new ValidationFailure("Email", ValidationFailureType.Missing));
            else
            {
                try
                {
                    _readOnlyRepsitory.First<UserEmailLogin>(x => x.Email == command.Email);
                }
                catch (ItemNotFoundException<UserEmailLogin>)
                {
                    validationFailures.Add(new ValidationFailure("Email", ValidationFailureType.DoesNotExist));
                }
            }

            if (validationFailures.Any())
                throw new CommandValidationException(validationFailures);
        }
    }
}