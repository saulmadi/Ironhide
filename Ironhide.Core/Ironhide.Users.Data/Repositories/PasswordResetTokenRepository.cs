using System;
using System.Threading.Tasks;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.App.Data.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        readonly IAppDataContext _dataContext;

        public PasswordResetTokenRepository(IAppDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Sample> GetById(Guid tokenId)
        {
            return await _dataContext.PasswordResetTokens.FindAsync(tokenId);
        }

        public Task Delete(Guid tokenId)
        {
            throw new NotImplementedException();
        }

        public Task Create(Sample passwordResetToken)
        {
            throw new NotImplementedException();
        }
    }
}