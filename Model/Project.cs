using Project_Manager.Enum;
using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Model
{
    public class Project : StatusDateTime
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Overview { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }

        public Guid UpdatedBy { get; set; }

        public Progress Progress { get; set; }

        public Complexity Complexity { get; set; }

        //foreign key for personal projects
        public User? Creator { get; set; }

        public ICollection<Issue>? Issues { get; set; }

        public ICollection<Wiki>? Wiki { get; set; }

        public Group? Group { get; set; }

        public Guid? OrganizationId { get; set; }
    }
}
