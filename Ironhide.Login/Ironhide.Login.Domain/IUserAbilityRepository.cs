using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ironhide.Login.Domain.Entities;

namespace Ironhide.Login.Domain
{
    public interface IUserAbilityRepository
    {
        Task<UserAbility> GetById(Guid id);
        Task<IEnumerable<UserAbility>> GetAll();
    }
}