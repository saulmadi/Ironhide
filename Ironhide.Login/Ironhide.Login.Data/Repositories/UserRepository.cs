using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly ILoginDataContext _dataContext;

        public UserRepository(ILoginDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<TUser> First<TUser>(Expression<Func<TUser, bool>> query) where TUser : User
        {
            return _dataContext.Users.OfType<TUser>().FirstAsync(query);
        }

        public async Task<TUser> GetById<TUser>(Guid id) where TUser : User
        {
            return await _dataContext.Users.OfType<TUser>().FirstAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<TUser>> Query<TUser>(Expression<Func<TUser, bool>> expression) where TUser : User
        {
            return await _dataContext.Users.OfType<TUser>().Where(expression).ToListAsync();
        }

    }
}