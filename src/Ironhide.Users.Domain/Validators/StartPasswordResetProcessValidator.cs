using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Exceptions;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Validators
{
    public class StartPasswordResetProcessValidator : ICommandValidator<IUserSession, StartPasswordResetProcess>
    {
        readonly IUserRepository _readOnlyRepsitory;

        public StartPasswordResetProcessValidator(IUserRepository readOnlyRepsitory)
        {
            _readOnlyRepsitory = readOnlyRepsitory;
        }

        public async Task Validate(IUserSession userSession, StartPasswordResetProcess command)
        {
            var validationFailures = new List<ValidationFailure>();

            if (string.IsNullOrEmpty(command.Email))
                validationFailures.Add(new ValidationFailure("Email", ValidationFailureType.Missing));
            else
            {
                try
                {
                    await _readOnlyRepsitory.First<UserEmailLogin>(x => x.Email == command.Email);
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