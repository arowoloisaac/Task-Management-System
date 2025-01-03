using Project_Manager.Model;

namespace Project_Manager.Service.GroupUserServices
{
    public interface IGroupUserService
    {
        Task AddUserToGroup(Guid organizationId, Guid groupId, Guid userId, Guid roleId);

        Task RemoveUserFromGroup(Guid organizationId, Guid groupId, Guid userId);

        Task RetrieveGroupUsers(Guid organizationId, Guid groupId);
    }
}
