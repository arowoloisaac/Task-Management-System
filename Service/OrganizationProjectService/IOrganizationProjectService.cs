using Project_Manager.DTO.OrganizationProjectDto;
using Project_Manager.DTO.ProjectDto;
using Project_Manager.Enum;

namespace Project_Manager.Service.OrganizationProjectService
{
    public interface IOrganizationProjectService
    {
        Task<string> CreateProject(CreateDto dto, Guid organizationId, Guid? groupId, string userId);

        Task<string> AssignProjectToGroup(Guid organizationId, Guid groupId, Guid projectId, string userId);

        Task<string> UnassignProjectToGroup(Guid organizationId, Guid groupId, Guid projectId);

        Task<string> UpdateProject(Guid projectId, string? name, string? description, Progress? progress, Complexity? complexity, Guid organizationId, string mail);

        Task<string> DeleteProject(Guid projectId, Guid organizationId, string userId);

        Task<string> EditProject(Guid projectId, UpdateProjectDto dto, Guid organizationId, string userId);

        Task<GetOrganizationProjectDto> GetProjectById(Guid projectId, string userId, Guid organizationId, Guid? groupId);

        //for the admin page
        Task<IEnumerable<GetOrganizationProjectDto>> GetProjects(Progress? progress, Complexity? complexity, bool? isAssigned, Guid organizationId, string userId);

        Task<IEnumerable<GetProjectDto>> GetGroupProjects(Guid organizationId, Guid groupId, string userId);

        Task<OrganizationProjectResponse> GetProjectPaginated(Progress? progress, Complexity? complexity, int? page, int itemPerPage, Guid organizationId, Guid groupId, string userId);
    }
}
