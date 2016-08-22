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
        readonly IUserRepository<UserEmailLogin> _repo;
        readonly IPasswordResetTokenRepository _tokenReadRepo;

        public PasswordResetter(IUserRepository<UserEmailLogin> repo,
            IPasswordResetTokenRepository tokenReadRepo)
        {
            _repo = repo;
            _tokenReadRepo = tokenReadRepo;
        }

        public async Task Handle(IUserSession userIssuingCommand, ResetPassword command)
        {
            PasswordResetToken passwordResetToken = await _tokenReadRepo.GetById(command.ResetPasswordToken);
            UserEmailLogin user = await _repo.GetById(passwordResetToken.UserId);
            user.ChangePassword(command.EncryptedPassword);
            await _repo.Update(user);
            await _tokenReadRepo.Delete(command.ResetPasswordToken);
            NotifyObservers(new PasswordReset(passwordResetToken.UserId));
        }

        public event DomainEvent NotifyObservers;
    }
}