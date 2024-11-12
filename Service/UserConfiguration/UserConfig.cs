using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Model;

namespace Project_Manager.Service.UserConfiguration
{
    public class UserConfig : IUserConfig
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserConfig(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<User> GetUser(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);

            if (user == null)
            {
                throw new ArgumentNullException("User is null");
            }
            
            return user;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new ArgumentNullException("User is null");
            }
            return user;
        }

        public async Task<OrganizationUser> ValidateOrganizationAdmin(string mail, Guid organizationId, string roleName)
        {
            var getRole = await _roleManager.FindByNameAsync(roleName);

            if (getRole == null)
            {
                throw new ArgumentNullException();
            }

            var user = await GetUser(mail);

            var organization = await _context.Organizations
                .SingleOrDefaultAsync(org => org.Id == organizationId);

            if(organization == null)
            {
                throw new Exception("Organization does not exist");
            }
            else
            {
                var organizationAdmin = await _context.OrganizationUser
                    .Where(u => u.Role == getRole && u.User == user && u.Organization == organization)
                    .SingleOrDefaultAsync();

                if (organizationAdmin == null)
                {
                    throw new Exception("You don't have the ability to perform this action");
                }

                return organizationAdmin;
            }
        }
    }
}
