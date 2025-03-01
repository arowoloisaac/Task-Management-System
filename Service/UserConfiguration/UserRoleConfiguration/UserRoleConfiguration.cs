using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Model;

namespace Project_Manager.Service.UserConfiguration.UserRoleConfiguration
{
    public class UserRoleConfiguration : IUserRoleConfiguration
    {
        private readonly ApplicationDbContext context;

        public UserRoleConfiguration(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Role> GetOrganizationRole(Guid userId, Guid organizationId)
        {
            var role = await context.OrganizationUser
                         .Include(uor => uor.Role)
                         .Where(uor => uor.User.Id == userId && uor.Organization.Id == organizationId)
                         .Select(uor => uor.Role)
                         .FirstOrDefaultAsync();

            if (role == null)
            {
                throw new Exception("Role does not exist");
            }
            return role;
        }

        public async Task<Role> GetGroupRole(Guid userId, Guid groupId)
        {
            var role = await context.GroupUsers
                         .Include(uor => uor.Role)
                         .Where(uor => uor.User.Id == userId && uor.Group.Id == groupId)
                         .Select(uor => uor.Role)
                         .FirstOrDefaultAsync();

            if (role == null)
            {
                throw new Exception("Role does not exist");
            }
            return role;
        }


        public async Task<IEnumerable<Role>> GetUserRoles(Guid userId, Guid? organizationId, Guid? groupId)
        {
            var roles = new List<Role>();

            if (organizationId.HasValue)
            {
                var orgRole = await GetOrganizationRole(userId, organizationId.Value);
                if (orgRole != null) roles.Add(orgRole);
            }

            if (groupId.HasValue)
            {
                var groupRole = await GetGroupRole(userId, groupId.Value);
                if (groupRole != null) roles.Add(groupRole);
            }

            return roles;
        }
    }
}
