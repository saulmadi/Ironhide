using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Unicron.Users.Domain.Entities;

namespace Unicron.Data
{
    public class UserLoginSessionAutoMappingOverride : IAutoMappingOverride<UserLoginSession>
    {
        public void Override(AutoMapping<UserLoginSession> mapping)
        {            
        }
    }
}