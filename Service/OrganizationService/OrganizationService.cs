using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Project_Manager.Configuration;
using Project_Manager.Data;
using Project_Manager.DTO.OrganizationDto;
using Project_Manager.Enum;
using Project_Manager.Model;
using Project_Manager.Service.UserConfiguration;

namespace Project_Manager.Service.OrganizationService
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserConfig _userConfig;
        private readonly RoleManager<Role> _roleManager;

        public string role = ApplicationRoleNames.OrganizationAdministrator;

        public OrganizationService(ApplicationDbContext context, UserManager<User> userManager, IUserConfig userConfig, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _userConfig = userConfig;
            _roleManager = roleManager;
        }

        public async Task<string> CreateOrganization(CreateOrganizationDto dto, string mail)
        {
            var user = await _userConfig.GetUser(mail);

            string organizationName = dto.Name;

            if(string.IsNullOrEmpty(organizationName))
            {
                throw new Exception("The name field needs a value");
            }

            var organization = await _context.Organizations.Where(org => org.Name == organizationName && org.CreatedBy == user.Id).SingleOrDefaultAsync();

            if(organization != null)
            {
                throw new Exception("This organization already exist");
            }
            else
            {
                var addOrganization = new Organization
                {
                    Id = Guid.NewGuid(),
                    Name = organizationName,
                    Description = dto.Description,
                    CreatedTime = DateTime.UtcNow,
                    CreatedBy = user.Id,
                };

                var response = await _context.Organizations.AddAsync(addOrganization);
                await _context.SaveChangesAsync();

                

                var savedOrg = await _context.Organizations
                    .Where(org => org.Name == organizationName && org.CreatedBy == user.Id)
                    .SingleOrDefaultAsync();

                if(savedOrg == null)
                {
                    throw new Exception("Uyou don't have any orgaization here");
                }

                var getRole = await _roleManager.FindByNameAsync(role);
                if(getRole != null)
                {
                    var checkRole = await _userManager.IsInRoleAsync(user, role);
                    if (checkRole == false)
                    {
                        var addToRole = await _userManager.AddToRoleAsync(user, role);
                        if (addToRole.Succeeded)
                        {
                            await _context.OrganizationUser.AddAsync(new OrganizationUser
                            {
                                Id = Guid.NewGuid(),
                                Role = getRole,
                                User = user,
                                Organization = savedOrg
                            });
                        }
                    }
                    else
                    {
                        await _context.OrganizationUser.AddAsync(new OrganizationUser
                        {
                            Id = Guid.NewGuid(),
                            Role = getRole,
                            User= user,
                            Organization= savedOrg
                        });
                    }
                }
                await _context.SaveChangesAsync();

                return response.ToString();
            }
        }

        public async Task<string> DeleteOrganization(Guid organizationId, string mail)
        {
            var user = await _userConfig.GetUser(mail);
            /***
             * inject the validate user role here
             * remove the validate org
             * then update the getOrg var by including the org and finding the createdby the user***/
            var validateOrg = await _context.OrganizationUser
                .Where(org => org.Role.Name == role && org.User.Id == user.Id && org.Organization.Id == organizationId)
                .FirstOrDefaultAsync();

            if (validateOrg == null)
            {
                throw new Exception("Either permssion or invalid Inputs");
            }
            else
            {
                /*var getOrg = await _context.Organizations
                    .Where(org => org.CreatedBy == validateOrg.User.Id && org.Id == validateOrg.Organization.Id)
                    .FirstOrDefaultAsync();*/

                var getOrg = await _context.Organizations
                    .Include(uo => uo.Users)
                    .FirstOrDefaultAsync(org => org.Id == organizationId);

                if (getOrg == null)
                {
                    throw new Exception("");
                }

                _context.Organizations.Remove(getOrg);
                _context.OrganizationUser.RemoveRange(validateOrg);

                await _context.SaveChangesAsync();
                return "Delete successfully";
            }
        }

        public Task<string> UpdateOrganization(Guid organizationId, string mail, UpdateOrganizationDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
