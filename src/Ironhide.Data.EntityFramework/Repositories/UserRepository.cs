using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ironhide.Users.Domain.Entities;
using Ironhide.Users.Domain.Services;

namespace Ironhide.Users.Data.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        readonly IUserDataContext _dataContext;

        public UserRepository(IUserDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<User> First(Expression<Func<User, bool>> query)
        {
            return _dataContext.Users.FirstAsync(query);
        }

        public async Task<User> GetById(Guid id)
        {
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> Query(Expression<Func<User, bool>> expression)
        {
            return await _dataContext.Users.Where(expression).ToListAsync();
        }
    }
}