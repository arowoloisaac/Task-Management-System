 using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Configuration;
using Project_Manager.Data;
using Project_Manager.DTO.OrganizationDto;
using Project_Manager.DTO.RequestDto;
using Project_Manager.Enum;
using Project_Manager.Model;
using Project_Manager.Service.UserConfiguration;
using Project_Manager.Service.UserOrganizationService;
using System.Data;

namespace Project_Manager.Service.OrganizationUserService
{
    public class OrganizationUserService : IOrganizationUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserConfig _userConfig;

        public string AdminRole = ApplicationRoleNames.OrganizationAdministrator;
        public const string MemberRole = ApplicationRoleNames.OrganizationMember;

        public OrganizationUserService( ApplicationDbContext context, IUserConfig userConfig)
        {
            _context =context;
            _userConfig = userConfig;
        }

        public async Task<string> AcceptOrganizationRequest(Guid organizationId, string userEmail)
        {
            try
            {
                var user = await _userConfig.GetUser(userEmail);

                var retrieveOrg = await _context.Organizations.FindAsync(organizationId);

                if (retrieveOrg == null)
                {
                    throw new Exception("Organization does not exist");
                }

                var retriveRequest = await _context.Requests.Where(req => req.OrganizationId == organizationId && req.UserId == user.Id).FirstOrDefaultAsync();

                if (retriveRequest == null)
                {
                    throw new Exception("No such request");
                }
                else
                {
                    var retrieveRole = await _userConfig.GetRole(MemberRole);
                    var isInRole = await _userConfig.UserInRole(MemberRole, user);
                    OrganizationUser requestToModel;

                    if (isInRole == true)
                    {
                        requestToModel = new OrganizationUser
                        {
                            Id = Guid.NewGuid(),
                            User = user,
                            Organization = retrieveOrg,
                            Role = retrieveRole,
                        };
                        await _context.OrganizationUser.AddAsync(requestToModel);
                    }
                    else
                    {
                        var addToRole = await _userConfig.AddUserToRole(MemberRole, user);
                        if (addToRole.Succeeded)
                        {
                            requestToModel = new OrganizationUser
                            {
                                Id = Guid.NewGuid(),
                                User = user,
                                Organization = retrieveOrg,
                                Role = retrieveRole,
                            };
                            await _context.OrganizationUser.AddAsync(requestToModel);
                        }

                    }

                    await _context.SaveChangesAsync();
                    return " User joined the organization successfully";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //function name should be send request to user
        public async Task<string> SendOrganizationRequest(Guid organizationId, string inviteeEmail, string adminId)
        {
            try
            {
                var adminUser = await _userConfig.ValidateOrganizationUser(adminId, organizationId, AdminRole);

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

        public async Task<string> RevokeOrganizationRequest(Guid organizationId, string receiver, string adminId)
        {
            try
            {
                var adminUser = await _userConfig.ValidateOrganizationUser(adminId, organizationId, AdminRole);

                if (adminUser == null)
                {
                    throw new Exception("Unable to validate user");
                }
                else
                {
                    var retrieveInvitee = await _userConfig.GetUser(receiver);

                    var checkIfOrgExist = await _context.Organizations
                        .Where( org => org.Id == organizationId)
                        .SingleOrDefaultAsync();

                    if (checkIfOrgExist == null)
                    {
                        throw new Exception("Organition does not exist");
                    }

                    var sentRequest = await _context.Requests
                            .SingleOrDefaultAsync(req => req.UserId == retrieveInvitee.Id && req.OrganizationId == organizationId);

                    if (sentRequest == null)
                    {
                        throw new Exception("Request does not exist");
                    }

                    _context.Requests.Remove(sentRequest);
                    await _context.SaveChangesAsync();

                    return "Removed Successfully";
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

        //this service helps retrieve organization user either belongs to or created by them
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

        public async Task<IEnumerable<OrganizationUserDto>> OrganizationUsers(Guid organizationId, string userEmail)
        {
            var user = await _userConfig.GetUser(userEmail);
            var userExistInOrg = await _context.OrganizationUser
                .SingleOrDefaultAsync(usr => usr.User.Email == user.Email && usr.Organization.Id == organizationId);

            if (user == null || userExistInOrg == null)
            {
                throw new Exception("User doesn't exist");
            }
            else
            {
                var usersInOrg = await _context.OrganizationUser
                    .Include(users => users.User)
                    .Where(org => org.Organization.Id == organizationId).ToListAsync();

                if (usersInOrg is null)
                {
                    return new List<OrganizationUserDto>();
                }

                var users = usersInOrg.Select(orgUsers => new OrganizationUserDto
                {
                    UserId = orgUsers.User.Id,
                    UserName = orgUsers.User.FirstName + " " + orgUsers.User.LastName,
                    UserEmail = orgUsers.User.Email!,
                    UserRole = orgUsers.Role?.Name!,
                }).ToList();

                return users;
            }
        }

        public async Task<string> RejectOrganizationRequest(Guid organizationId, string userEmail)
        {
            try
            {
                var user = await _userConfig.GetUser(userEmail);

                var retrieveOrg = await _context.Organizations.FindAsync(organizationId);

                if (retrieveOrg == null)
                {
                    throw new Exception("Organization does not exist");
                }

                var retrieveRole = await _userConfig.GetRole(userEmail);

                var retriveRequest = await _context.Requests
                    .Where(req => req.OrganizationId == organizationId && req.UserId == user.Id)
                    .SingleOrDefaultAsync();

                if (retriveRequest == null)
                {
                    throw new Exception("No such request");
                }

                _context.Requests.Remove(retriveRequest);

                await _context.SaveChangesAsync();

                return "Successfully rejected";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> RemoveUserFromOrganization(Guid organizationId, string memberMail, string adminId)
        {
            try
            {
                var adminUser = await _userConfig.ValidateOrganizationUser(adminId, organizationId, AdminRole);

                if (adminUser == null)
                {
                    throw new Exception("Unable to validate user");
                }
                else
                {
                    var member = await _userConfig.GetUser(memberMail);

                    var checkIfUserOrgExist = await _context.OrganizationUser
                        .Where(u => u.User == member && u.Organization.Id == organizationId && u.Organization.CreatedBy != adminUser.Id)
                        .SingleOrDefaultAsync();

                    if (member == null || checkIfUserOrgExist == null)
                    {
                        throw new Exception("This user does not exist in the system");
                    }

                    else
                    {
                        _context.OrganizationUser.Remove(checkIfUserOrgExist);
                    }
                    await _context.SaveChangesAsync();
                    return "user removed from the organization";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<InvitationRequestDto>> InvitationList(string mail)
        {
            var user = await _userConfig.GetUser(mail);

            var response = await (from req in _context.Requests
                                  join org in _context.Organizations
                                  on req.OrganizationId equals org.Id
                                   where req.UserId == user.Id
                                  select new InvitationRequestDto
                                  {
                                      OrganizationId = req.OrganizationId,
                                      OrganizationName = org.Name
                                  }).ToListAsync();

            return response;
        }

        public Task<IEnumerable<SentRequestDto>> SentRequests(Guid organizationId)
        {
            throw new NotImplementedException();
        }
    }
}
