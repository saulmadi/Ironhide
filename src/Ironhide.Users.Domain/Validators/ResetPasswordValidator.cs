﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Common;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Exceptions;

namespace Ironhide.Users.Domain.Validators
{
    public class ResetPasswordValidator : ICommandValidator<IUserSession, ResetPassword>
    {
        readonly IPasswordResetTokenRepository _readOnlyRepo;
        readonly ITimeProvider _timeProvider;

        public ResetPasswordValidator(IPasswordResetTokenRepository readOnlyRepo,
            ITimeProvider timeProvider)
        {
            _readOnlyRepo = readOnlyRepo;
            _timeProvider = timeProvider;
        }

        public async Task Validate(IUserSession userSession, ResetPassword command)
        {
            var failures = new List<ValidationFailure>();
            if (command.EncryptedPassword == null || string.IsNullOrEmpty(command.EncryptedPassword.Password))
            {
                failures.Add(new ValidationFailure("EncryptedPassword", ValidationFailureType.Missing));
            }
            if (command.ResetPasswordToken == Guid.Empty)
            {
                failures.Add(new ValidationFailure("ResetPasswordToken", ValidationFailureType.Missing));
            }
            else
            {
                try
                {
                    PasswordResetToken passwordResetToken =
                        await _readOnlyRepo.GetById(command.ResetPasswordToken);

                    if (passwordResetToken.Created > _timeProvider.Now().AddDays(2))
                    {
                        failures.Add(new ValidationFailure("ResetPasswordToken", ValidationFailureType.Expired));
                    }
                }
                catch (ItemNotFoundException<PasswordResetToken>)
                {
                    failures.Add(new ValidationFailure("ResetPasswordToken", ValidationFailureType.DoesNotExist));
                }
            }
            if (failures.Any())
                throw new CommandValidationException(failures);
        }
    }
}