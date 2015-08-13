using System.Collections.Generic;

namespace Ironhide.Web.Api.Requests
{
    public class NewUserRequest
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public IEnumerable<UserAbilityRequest> Abilities { get; set; }
    }
}