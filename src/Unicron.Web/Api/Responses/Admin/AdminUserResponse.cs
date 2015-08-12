using System;

namespace Unicron.Web.Api.Responses.Admin
{
    public class AdminUserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
    }
}