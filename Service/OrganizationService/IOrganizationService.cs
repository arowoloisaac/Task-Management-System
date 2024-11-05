using Project_Manager.DTO.OrganizationDto;
using Project_Manager.Enum;

namespace Project_Manager.Service.OrganizationService
{
    public interface IOrganizationService
    {
        Task<string> CreateOrganization(CreateOrganizationDto dto, string mail);

        Task<string> DeleteOrganization(Guid organizationId, string mail);

        Task<GetOrganizationDto> GetOrganization(Guid organizationId, string mail);

        Task<string> UpdateOrganization(Guid organizationId, string mail, UpdateOrganizationDto dto);

        //Task<string> GetOrganizationUsers(Guid organizationId);

        Task<IEnumerable<GetOrganizationDto>> GetOrganizations(OrganizationFilter filter, string userMail);
    }
}
