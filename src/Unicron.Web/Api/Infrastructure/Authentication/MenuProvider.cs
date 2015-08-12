using System.Collections.Generic;
using System.Linq;
using Unicron.Web.Api.Infrastructure.Authentication.Roles;
using Unicron.Web.Api.Infrastructure.Exceptions;

namespace Unicron.Web.Api.Infrastructure.Authentication
{
    public class MenuProvider:IMenuProvider
    {
        private readonly IEnumerable<UsersRoles> _usersRoles;

        public MenuProvider(IEnumerable<UsersRoles> usersRoles)
        {
            _usersRoles = usersRoles;
        }

        public string[] getFeatures(string claim)
        {
            var firstOrDefault = _usersRoles.FirstOrDefault(x => x.Name.Equals(claim));
            if (firstOrDefault != null)
                return firstOrDefault.Features.Select(y => y.Description).ToArray();

            throw new RolNotFound(claim);
        }

        public string[] getFeatures(string[] claims)
        {
            var result = claims.SelectMany(getFeatures);

            return result.ToArray();
        }

        public string[] getAllFeatures()
        {
          
            return _usersRoles.SelectMany(x => x.Features).Select(x => x.Description).ToArray();

        }
    }
}