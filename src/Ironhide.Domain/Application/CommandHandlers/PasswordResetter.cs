using System.Threading.Tasks;
using AcklenAvenue.Commands;
using Ironhide.Users.Domain.Application.Commands;
using Ironhide.Users.Domain.DomainEvents;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Domain.Application.CommandHandlers
{
    public class PasswordResetter : IEventedCommandHandler<IUserSession, ResetPassword>
    {
        readonly IPasswordResetTokenRepository _tokenReadRepo;
        readonly IUserRepository<UserEmailLogin> _userReadRepo;
        readonly IWriteableRepository _writeableRepository;

        public PasswordResetter(IUserRepository<UserEmailLogin> userReadRepo,
            IPasswordResetTokenRepository tokenReadRepo, IWriteableRepository writeableRepository)
        {
            _userReadRepo = userReadRepo;
            _tokenReadRepo = tokenReadRepo;
            _writeableRepository = writeableRepository;
        }

        public async Task Handle(IUserSession userIssuingCommand, ResetPassword command)
        {
            PasswordResetToken passwordResetToken = await _tokenReadRepo.GetById(command.ResetPasswordToken);
            UserEmailLogin user = await _userReadRepo.GetById(passwordResetToken.UserId);
            user.ChangePassword(command.EncryptedPassword);
            await _writeableRepository.Update(user);
            await _writeableRepository.Delete<PasswordResetToken>(command.ResetPasswordToken);
            NotifyObservers(new PasswordReset(passwordResetToken.UserId));
        }

        public event DomainEvent NotifyObservers;
    }
}