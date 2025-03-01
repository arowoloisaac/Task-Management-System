using Project_Manager.Enum;
using System.ComponentModel.DataAnnotations;

namespace Project_Manager.DTO.TaskDto
{
    public class RetrieveIssue
    {
        public Guid id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Complexity Complexity { get; set; }

        public IssueType IssueType { get; set; }

        public Progress Progress { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}
