using Project_Manager.Enum;

namespace Project_Manager.DTO.ProjectDto
{
    public class CreateDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Complexity Complexity { get; set; }
    }
}
