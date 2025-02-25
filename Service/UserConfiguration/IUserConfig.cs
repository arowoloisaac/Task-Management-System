using Microsoft.AspNetCore.Identity;
using Project_Manager.Model;

namespace Project_Manager.Service.UserConfiguration
{
    public interface IUserConfig
    {
        Task<User> GetUser(string mail); 

        Task<User> GetUserById(string id);

        Task<OrganizationUser> ValidateOrganizationUser(string mail,Guid organizationId, string roleName);

        Task<Role> GetRole(string roleName);

        Task<bool> UserInRole(string roleName, User user);

        Task<IdentityResult> AddUserToRole(string roleName, User user);
    }
}
