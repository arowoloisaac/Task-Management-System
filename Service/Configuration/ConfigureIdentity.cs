using Microsoft.AspNetCore.Identity;
using Project_Manager.Configuration;
using Project_Manager.Model;

namespace Project_Manager.Service.Configuration
{
    public static class ConfigureIdentity
    {
        public static async Task ConfigureIdentityAsync(this WebApplication app)
        {
            using var serviceScope =  app.Services.CreateScope();

            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            string[] roles = new string[] 
            { 
                ApplicationRoleNames.OrganizationAdministrator,
                ApplicationRoleNames.OrganizationCurator,
                ApplicationRoleNames.OrganizationMember,
                ApplicationRoleNames.GroupAdministrator,
                ApplicationRoleNames.GroupUser,
                ApplicationRoleNames.GroupModerator
            };

            foreach (var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Role
                    {
                        Name = role,
                    });
                }
            }
        }
    }
}
