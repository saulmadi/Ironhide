using System;

namespace Ironhide.Api.Modules.UserManagement
{
    public class AdminUpdateUserRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}