using Project_Manager.DTO.OrganizationDto;
using Project_Manager.Model;

namespace Project_Manager.Service.GroupUserServices
{
    public interface IGroupUserService
    {
        Task<string> AddUserToGroup(Guid organizationId, Guid groupId, string userEmail, string roleName);

        Task<string> RemoveUserFromGroup(Guid organizationId, Guid groupId, string userEmail);

        Task<IEnumerable<GroupUserDto>> RetrieveGroupUsers(Guid organizationId, Guid groupId);

        Task<string> UpdateUserRole(Guid organizationId, Guid groupId, string roleName, string userToUpdate);

        Task<GroupUserDto> RetrieveUser(Guid organizationId, Guid groupId, string userId);
    }
}
