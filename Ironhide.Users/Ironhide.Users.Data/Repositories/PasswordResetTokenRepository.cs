using System;
using System.Threading.Tasks;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Data.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        readonly IUserDataContext _dataContext;

        public PasswordResetTokenRepository(IUserDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<PasswordResetToken> GetById(Guid tokenId)
        {
            return await _dataContext.PasswordResetTokens.FindAsync(tokenId);
        }

        public Task Delete(Guid tokenId)
        {
            throw new NotImplementedException();
        }

        public Task Create(PasswordResetToken passwordResetToken)
        {
            throw new NotImplementedException();
        }
    }
}