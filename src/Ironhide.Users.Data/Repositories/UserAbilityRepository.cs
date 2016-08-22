using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Data.Repositories
{
    public class UserAbilityRepository : IUserAbilityRepository
    {
        readonly IUserDataContext _dataContext;

        public UserAbilityRepository(IUserDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<UserAbility> GetById(Guid id)
        {
            return await _dataContext.UserAbilities.FindAsync(id);
        }

        public async Task<IEnumerable<UserAbility>> GetAll()
        {
            return await _dataContext.UserAbilities.ToListAsync();
        }
    }
}