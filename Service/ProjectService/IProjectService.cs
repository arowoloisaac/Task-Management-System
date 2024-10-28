using Project_Manager.DTO.ProjectDto;
using Project_Manager.Enum;

namespace Project_Manager.Service.ProjectService
{
    public interface IProjectService
    {
        //to update this by addsing a boolean later on isPersonnal then add conditional statement to the service methods
        Task<string> CreateProject(CreateDto dto, string mail);

        Task<string> UpdateProject(Guid projectId, string? Name, string? Description, Progress? progress, Complexity? complexity, string mail);

        Task<string> DeleteProject(Guid projectId, string mail);

        Task<GetProjectDto> GetProjectById(Guid projectId, string mail);

        Task<IEnumerable<GetProjectDto>> GetProjects(Progress? progress, Complexity? complexity, string mail);
        
    }
}
