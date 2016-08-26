using System;

namespace Ironhide.App.Domain.Services
{
    public class GuidIdentityGenerator : IIdentityGenerator<Guid>
    {
        public Guid Generate()
        {
            return Guid.NewGuid();
        }
    }
}