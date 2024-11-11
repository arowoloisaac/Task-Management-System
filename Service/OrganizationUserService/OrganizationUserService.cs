using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Configuration;
using Project_Manager.Data;
using Project_Manager.DTO.OrganizationDto;
using Project_Manager.Enum;
using Project_Manager.Model;
using Project_Manager.Service.UserConfiguration;
using Project_Manager.Service.UserOrganizationService;

namespace Project_Manager.Service.OrganizationUserService
{
    public class OrganizationUserService : IOrganizationUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserConfig _userConfig;

        public string AdminRole = ApplicationRoleNames.OrganizationAdministrator;

        public OrganizationUserService( ApplicationDbContext context, IUserConfig userConfig)
        {
            _context =context;
            _userConfig = userConfig;
        }

        public async Task<string> AddUserToOrganization(Guid organization, string invitee, string adminId)
        {
            var adminUser = await _userConfig.ValidateOrganizationUser(adminId, organization, AdminRole);

            /***
             * todo later on
             * validate the receiver
             * add the invitee to the request table
             * send an email to the invitee
             * if the invitee accepts the request 
             * then add them to the organization then remove the user from the request
             * else remove then from the request table
             * ***/
            throw new Exception();
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

            if (organizationList.Count < 1)
            {
                return new List<GetOrganizationDto>();
            }

            var creatorIds = organizationList
                .Select(o => o.Organization.CreatedBy).Distinct().ToList();

            var creators = await _context.Users
                .Where(u => creatorIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => new { u.Email, u.FirstName });


            var mapOrg = organizationList.Select(check => new GetOrganizationDto
            {
                Id = check.Organization.Id,
                Name = check.Organization.Name,
                Role = check.Role.Name,
                Creator = creators[check.Organization.CreatedBy].Email == user.Email ? "created by you" : "Joined"
            }).ToList();

            return mapOrg;
        }
    }
}
