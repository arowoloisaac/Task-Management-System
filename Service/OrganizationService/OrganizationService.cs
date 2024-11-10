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

            var validateOrg = await _context.OrganizationUser
                .Where(org => org.Role.Name == role && org.User.Id == user.Id && org.Organization.Id == organizationId)
                .FirstOrDefaultAsync();

            if (validateOrg == null)
            {
                throw new Exception("Either permssion or invalid Inputs");
            }
            else
            {
                var getOrg = await _context.Organizations
                    .Where(org => org.CreatedBy == validateOrg.User.Id && org.Id == validateOrg.Organization.Id)
                    .FirstOrDefaultAsync();

                if (getOrg == null)
                {
                    throw new Exception("");
                }

                _context.Remove(getOrg);
                _context.Organizations.Remove(getOrg);
                _context.OrganizationUser.RemoveRange(validateOrg);

                await _context.SaveChangesAsync();
                return "Delete successfully";
            }
            /***
             * check for the role first in the userorganization role
             * check if the user is the creator of the orgaization
             * then remove everything here
             * from the implementation of project service where all the issues are being removed will be injected here***/
        }

        public async Task<GetOrganizationDto> GetOrganization(Guid organizationId, string mail)
        {
            var user = await _userConfig.GetUser(mail);

            var getSpecificOrganization = await _context.OrganizationUser
                .Include(user => user.Role)
                .Include(user => user.Organization)
                .Where(org => org.Organization.Id == organizationId && org.User == user)
                .SingleOrDefaultAsync();

            if (getSpecificOrganization == null)
            {
                throw new Exception("You don't belong to this organization");
            }
            else
            {
                var getOrg = new GetOrganizationDto
                {
                    Id = organizationId,
                    Name = getSpecificOrganization.Organization.Name,
                    Creator = getSpecificOrganization.User.UserName,
                    Description = getSpecificOrganization.Organization.Description,
                    Role = $"Your role - {getSpecificOrganization.Role.Name}"
                };

                return getOrg;
            }
        }

        public async Task<IEnumerable<GetOrganizationDto>> GetOrganizations(OrganizationFilter? filter, string mail)
        {
            var user = await _userConfig.GetUser(mail);

            IQueryable<OrganizationUser> query = _context.OrganizationUser
                .Where(u => u.User.Id == user.Id)
                .Include(org => org.Organization)
                .Include(role => role.Role);

            if (filter.HasValue)
            {
                if (filter.Value == OrganizationFilter.Created)
                {
                    query = query.Where(val => val.Organization.CreatedBy == user.Id);
                }
                else
                {
                    query = query.Where(val => val.Organization.CreatedBy != user.Id);
                }
            }
            var organizationList = await query.ToListAsync();

            if(organizationList.Count < 1)
            {
                return new List<GetOrganizationDto>(); 
            }

            var creatorIds = organizationList
                .Select(o => o.Organization.CreatedBy).Distinct().ToList();

            var creators = await _context.Users
                .Where(u => creatorIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => new { u.Email, u.FirstName });


            var mapOrg = organizationList.Select( check => new GetOrganizationDto
            {
                Id = check.Id,
                Name = check.Organization.Name,
                Role = check.Role.Name,
                Creator = creators[check.Organization.CreatedBy].Email == user.Email? "created by you" : "Joined"
            }).ToList();
            
            return mapOrg;
        }

        public Task<string> UpdateOrganization(Guid organizationId, string mail, UpdateOrganizationDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
