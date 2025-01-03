
namespace Project_Manager.Service.GroupUserServices
{
    public class GroupUserService : IGroupUserService
    {
        public async Task AddUserToGroup(Guid organizationId, Guid groupId, Guid userId, Guid roleId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveUserFromGroup(Guid organizationId, Guid groupId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task RetrieveGroupUsers(Guid organizationId, Guid groupId)
        {
            throw new NotImplementedException();
        }
    }
}
