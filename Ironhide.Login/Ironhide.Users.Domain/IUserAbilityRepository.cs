using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Domain
{
    public interface IUserAbilityRepository
    {
        Task<UserAbility> GetById(Guid id);
        Task<IEnumerable<UserAbility>> GetAll();
    }
}