using System;
using System.Collections.Generic;

namespace Unicron.Web.Api.Requests
{
    public class UserAbilitiesRequest
    {
        public IEnumerable<UserAbilityRequest> Abilities { get; set; }
        public Guid UserId { get; set; }
    }
}