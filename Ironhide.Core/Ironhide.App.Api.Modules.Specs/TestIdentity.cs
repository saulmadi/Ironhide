using System.Collections.Generic;
using System.Linq;
using Nancy.Security;

namespace Ironhide.App.Api.Modules.Specs
{
    public class TestIdentity : IUserIdentity
    {
        public TestIdentity(string username = "user", params string[] claims)
        {
            UserName = username;
            Claims = claims.ToList();
        }

        public string UserName { get; private set; }
        public IEnumerable<string> Claims { get; private set; }
    }
}