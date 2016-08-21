using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Domain
{
    public interface IRoleRepository
    {
        Task<Role> GetById(Guid roleId);
        Task<Role> First(Expression<Func<Role, bool>> func);
    }
}