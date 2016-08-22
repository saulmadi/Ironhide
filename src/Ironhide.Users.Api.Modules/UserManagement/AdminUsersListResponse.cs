using System.Collections.Generic;

namespace Ironhide.Api.Modules.UserManagement
{
    public class AdminUsersListResponse
    {
        public AdminUsersListResponse(List<AdminUserResponse> adminUsers)
        {
            AdminUsers = adminUsers;
        }

        public List<AdminUserResponse> AdminUsers { get; set; }
    }
}