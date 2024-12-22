using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Configuration;
using Project_Manager.Data;
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

        public static async Task ConfigureDefaultAvatar(this WebApplication app)
        {
            using var serviceScope = app.Services.CreateScope();

            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            string avatarUrl = "https://windsorchapel.org/wp-content/uploads/2015/05/blank_avatar_male1-e1452782680848.jpg";

            var avatarExist = await dbContext.Avatars.FirstOrDefaultAsync(url => url.AvatarUrl == avatarUrl);

            if (avatarExist != null)
            {
                return;
            }

            var newAvatar = new Avatar
            {
                Id = Guid.NewGuid(),
                AvatarUrl = avatarUrl,
            };

            await dbContext.Avatars.AddAsync(newAvatar);

            await dbContext.SaveChangesAsync();
        }
    }
}
