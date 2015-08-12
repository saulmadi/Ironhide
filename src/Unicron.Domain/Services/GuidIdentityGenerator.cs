using System;

namespace Unicron.Users.Domain.Services
{
    public class GuidIdentityGenerator : IIdentityGenerator<Guid>
    {
        public Guid Generate()
        {
            return Guid.NewGuid();
        }
    }
}