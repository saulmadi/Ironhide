using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Data
{
    public class UserLoginSessionAutoMappingOverride : IAutoMappingOverride<UserLoginSession>
    {
        public void Override(AutoMapping<UserLoginSession> mapping)
        {            
        }
    }
}