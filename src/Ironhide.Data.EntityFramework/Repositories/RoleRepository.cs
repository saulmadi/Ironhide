using System;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ironhide.Users.Domain;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        readonly IUserDataContext _dataContext;

        public RoleRepository(IUserDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Role> GetById(Guid roleId)
        {
            return await _dataContext.Roles.FindAsync(roleId);
        }

        public async Task<Role> First(Expression<Func<Role, bool>> func)
        {
            return await _dataContext.Roles.FirstAsync(func);
        }
    }
}