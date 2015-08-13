using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class PasswordResetter : ICommandHandler<ResetPassword>
    {
        readonly IReadOnlyRepository _readOnlyRepository;
        readonly IWriteableRepository _writeableRepository;

        public PasswordResetter(IReadOnlyRepository readOnlyRepository, IWriteableRepository writeableRepository)
        {
            _readOnlyRepository = readOnlyRepository;
            _writeableRepository = writeableRepository;
        }

        public void Handle(IUserSession userIssuingCommand, ResetPassword command)
        {
            var passwordResetToken = _readOnlyRepository.GetById<PasswordResetAuthorization>(command.ResetPasswordToken);
            var user = _readOnlyRepository.GetById<UserEmailLogin>(passwordResetToken.UserId);
            user.ChangePassword(command.EncryptedPassword);
            _writeableRepository.Update(user);
            _writeableRepository.Delete<PasswordResetAuthorization>(command.ResetPasswordToken);
            NotifyObservers(new PasswordReset(passwordResetToken.UserId));
        }

        public event DomainEvent NotifyObservers;
    }
}