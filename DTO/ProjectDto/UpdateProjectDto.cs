using Project_Manager.Enum;

namespace Project_Manager.DTO.ProjectDto
{
    public class UpdateProjectDto
    {
        public string? Name { get; set; }

        public string? Description { get; set; } 
         
        public string? Overview { get; set; }

        public Complexity? Complexity { get; set; }

        public Progress? Progress { get; set; }
    }
}
