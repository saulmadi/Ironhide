using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ironhide.Users.Domain.Services
{
    public interface IUserRepository<TUser> where TUser : User
    {
        Task<TUser> First(Expression<Func<TUser, bool>> query);
        Task<TUser> GetById(Guid id);
        Task<IEnumerable<TUser>> Query(Expression<Func<TUser, bool>> expression);
        Task Delete(Guid userId);
        Task Update(User user);
        Task<User> Create(User user);
    }
}