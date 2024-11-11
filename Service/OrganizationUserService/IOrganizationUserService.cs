using Project_Manager.DTO.OrganizationDto;
using Project_Manager.Enum;

namespace Project_Manager.Service.UserOrganizationService
{
    public interface IOrganizationUserService
    {
        Task<string> AddUserToOrganization(Guid organization, string receiver, string adminId);

        Task<GetOrganizationDto> GetOrganization(Guid organizationId, string mail);

        Task<IEnumerable<GetOrganizationDto>> GetOrganizations(OrganizationFilter? filter, string userMail);
    }
}
