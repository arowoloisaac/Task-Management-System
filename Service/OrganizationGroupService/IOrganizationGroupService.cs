using Project_Manager.DTO.GroupDto.OrganizationGrpDto;

namespace Project_Manager.Service.OrganizationProjectService
{
    public interface IOrganizationGroupService
    {
        /***
         * create group
         * delete group
         * update group
         * retrieve group
         ***/

        Task<string> CreateOrganizationGroup(string groupName, Guid organizationId, string mail);

        Task<string> UpdateOrganizationGroup(Guid groupId,string? groupName, Guid organizationId, string adminEmail);

        Task<string> DeleteOrganizationGroup(Guid groupId, Guid organizationId, string adminEmail);

        Task<IEnumerable<RetrieveGroupDto>> RetrieveOrganizationGroup(string adminMail, Guid organizationId);

        Task<RetrieveGroupDto> RetrieveOrganizationGroupById(Guid groupId, Guid organizationId, string mail);
    }
}
