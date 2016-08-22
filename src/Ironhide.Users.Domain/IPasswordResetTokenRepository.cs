using System;
using System.Threading.Tasks;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Domain
{
    public interface IPasswordResetTokenRepository
    {
        Task<PasswordResetToken> GetById(Guid tokenId);
        Task Delete(Guid tokenId);
        Task Create(PasswordResetToken passwordResetToken);
    }
}