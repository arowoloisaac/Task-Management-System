using Project_Manager.DTO.OrganizationDto;
using Project_Manager.DTO.RequestDto;
using Project_Manager.Enum;

namespace Project_Manager.Service.UserOrganizationService
{
    public interface IOrganizationUserService
    {
        //send request to the specific user by email
        Task<string> SendOrganizationRequest(Guid organization, string receiver, string adminId);

        Task<string> RevokeOrganizationRequest(Guid organization, string receiver, string adminId);

        Task<string> RemoveUserFromOrganization(Guid organization, string receiver, string adminId);

        //to get a specific organization
        Task<GetOrganizationDto> GetOrganization(Guid organizationId, string mail);

        Task<IEnumerable<GetOrganizationDto>> GetOrganizations(OrganizationFilter? filter, string userMail);

        Task<IEnumerable<OrganizationUserDto>> OrganizationUsers(Guid organizationId, string userEmail);

        Task<string> AcceptOrganizationRequest(Guid organizationId, string userEmail);

        Task<string> RejectOrganizationRequest(Guid organizationId, string userEmail);

        Task<IEnumerable<InvitationRequestDto>> InvitationList(string mail);

        Task<IEnumerable<SentRequestDto>> SentRequests(Guid organizationId);
    }
}
