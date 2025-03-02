using Project_Manager.DTO.ProjectDto;
using Project_Manager.Model;

namespace Project_Manager.DTO.OrganizationProjectDto
{
    public class OrganizationProjectResponse
    {
        public IEnumerable<GetOrganizationProjectDto> Projects { get; set; } = new List<GetOrganizationProjectDto>();

        public Pagination Pagination { get; set; }

        public OrganizationProjectResponse(List<GetOrganizationProjectDto> project, int page, int total, int count, int start, int end, int totalItem)
        {
            Projects = project;

            Pagination = new Pagination
            {
                Count = count,
                Current = page,
                Size = total,
                Start = start,
                End = end,
                TotalItems = totalItem
            };
        }
    }
}
