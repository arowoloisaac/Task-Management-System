using Project_Manager.DTO.ProjectDto;

namespace Project_Manager.Model
{
    public class ProjectResponse
    {
        public IEnumerable<GetProjectDto> Projects { get; set; } = new List<GetProjectDto>();

        public Pagination Pagination { get; set; }

        public ProjectResponse(List<GetProjectDto> project, int page, int total, int count, int start,int end)
        {
            this.Projects = project;

            this.Pagination = new Pagination
            {
                Count = count,
                Current = page,
                Size = total,
                Start = start,
                End = end
            };
        }
    }
}
