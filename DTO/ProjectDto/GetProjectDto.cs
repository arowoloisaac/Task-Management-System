namespace Project_Manager.DTO.ProjectDto
{
    public class GetProjectDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Creator { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; }
    }
}
