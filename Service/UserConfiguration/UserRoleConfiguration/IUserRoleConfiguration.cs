using Project_Manager.Model;

namespace Project_Manager.Service.UserConfiguration.UserRoleConfiguration
{
    public interface IUserRoleConfiguration
    {
        Task<Role?> GetOrganizationRole(Guid userId, Guid organizationId);

        Task<Role?> GetOrganizationRoleEmail(string userMail, Guid organizationId);

        Task<Role> GetGroupRole(Guid userId, Guid organizationId, Guid groupId);

        Task<Role?> GetGroupRoleByEmail(string userMail, Guid organizationId, Guid groupId);

        Task<IEnumerable<Role>> GetUserRoles(Guid userId, Guid organizationId, Guid? groupId);
    }
}
