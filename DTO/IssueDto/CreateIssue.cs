using Project_Manager.Enum;
using System.ComponentModel.DataAnnotations;

namespace Project_Manager.DTO.IssueDto
{
    public class CreateIssue
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public uint EstimatedTimeInMinutes { get; set; }

        [Required(ErrorMessage ="Complexity required")]
        public Complexity Complexity { get; set; }

        public IssueType IssueType { get; set; }

        //public Guid? AssignedTo { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}
