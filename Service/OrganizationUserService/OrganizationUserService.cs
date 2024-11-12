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

        public async Task<string> AddUserToOrganization(Guid organizationId, string inviteeEmail, string adminId)
        {
            try
            {
                var adminUser = await _userConfig.ValidateOrganizationAdmin(adminId, organizationId, AdminRole);

                if (adminUser == null)
                {
                    throw new Exception("Unable to validate user");
                }
                else
                {
                    var retrieveInvitee = await _userConfig.GetUser(inviteeEmail);

                    var checkIfUserOrgExist = await _context.OrganizationUser
                        .Where(u => u.User == retrieveInvitee && u.Organization.Id == organizationId)
                        .SingleOrDefaultAsync();

                    if (retrieveInvitee == null || retrieveInvitee.Email is null && checkIfUserOrgExist != null)
                    {
                        throw new Exception("This user does not exist in our system");
                    }

                    else
                    {
                        var request = await _context.Requests
                            .SingleOrDefaultAsync(req => req.UserId == retrieveInvitee.Id && req.OrganizationId == organizationId);

                        if (request != null)
                        {
                            throw new Exception("There is an existing request for this user for this organization");
                        }

                        var sendResponse = await _context.Requests.AddAsync(new Requests
                        {
                            Id = Guid.NewGuid(),
                            OrganizationId = organizationId,
                            InviteeEmail = retrieveInvitee.Email,
                            UserId = retrieveInvitee.Id
                        });
                    }
                    await _context.SaveChangesAsync();
                    return "Request sent to the user";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

        public async Task<string> RemoveUserFromOrganization(Guid organization, string receiver, string adminId)
        {
            throw new NotImplementedException();
        }

    }
}
