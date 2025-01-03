using Project_Manager.DTO.OrganizationDto;
using Project_Manager.Enum;

namespace Project_Manager.Service.UserOrganizationService
{
    public interface IOrganizationUserService
    {
        //send request to the specific user by email
        Task<string> AddUserToOrganization(Guid organization, string receiver, string adminId);

        Task<string> RemoveUserFromOrganization(Guid organization, string receiver, string adminId);

        //to get a specific organization
        Task<GetOrganizationDto> GetOrganization(Guid organizationId, string mail);

        Task<IEnumerable<GetOrganizationDto>> GetOrganizations(OrganizationFilter? filter, string userMail);

        Task<IEnumerable<OrganizationUserDto>> organizationUsers(Guid organizationId, string userEmsil);
    }
}
