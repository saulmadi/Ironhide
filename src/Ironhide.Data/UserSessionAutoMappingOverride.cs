using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Data
{
    public class UserSessionAutoMappingOverride : IAutoMappingOverride<UserLoginSession>
    {

        public void Override(AutoMapping<UserLoginSession> mapping)
        {
            mapping.Map(x => x.JwtToken).Length(1000);
        }
    }
}