using Project_Manager.Enum;

namespace Project_Manager.DTO.OrganizationProjectDto
{
    public class GetOrganizationProjectDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Overview { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Complexity Complexity { get; set; }

        public Progress Progress { get; set; }

        public DateTime DateCreated { get; set; }

        public string AssignedTo { get; set; } = string.Empty;

        public Guid? AssignedGroupId { get; set; }
    }
}
