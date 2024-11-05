using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public OrganizationService(ApplicationDbContext context, UserManager<User> userManager, IUserConfig userConfig)
        {
            _context = context;
            _userManager = userManager;
            _userConfig = userConfig;
        }

        public async Task<string> CreateOrganization(CreateOrganizationDto dto, string mail)
        {
            var user = await _userConfig.GetUser(mail);

            string organizationName = dto.Name;

            if(string.IsNullOrEmpty(organizationName))
            {
                throw new Exception("The name field needs a value");
            }

            var organization = await _context.Organizations.Where(org => org.Name == organizationName).SingleOrDefaultAsync();

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

                return response.ToString();
            }
        }

        public Task<string> DeleteOrganization(Guid organizationId, string mail)
        {
            throw new NotImplementedException();
        }

        public Task<GetOrganizationDto> GetOrganization(Guid organizationId, string mail)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetOrganizationDto>> GetOrganizations(OrganizationFilter filter, string userMail)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateOrganization(Guid organizationId, string mail, UpdateOrganizationDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
