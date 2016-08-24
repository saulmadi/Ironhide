using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Domain.Services
{
    public interface IUserRepository
    {
        Task<TUser> First<TUser>(Expression<Func<TUser, bool>> query) where TUser : User;
        Task<TUser> GetById<TUser>(Guid id) where TUser : User;
        Task<IEnumerable<TUser>> Query<TUser>(Expression<Func<TUser, bool>> expression) where TUser : User;
    }
}