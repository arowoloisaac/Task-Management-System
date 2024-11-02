using Project_Manager.Enum;
using System.ComponentModel.DataAnnotations;

namespace Project_Manager.Model
{
    public class Issue : ObjectDateTime
    {
        public Guid Id { get; set; }
        // has sub task bool+

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public IssueType IssueType { get; set; }

        public int IssueLevel { get; set; } = 0;

        public uint TimeSpent { get; set; } = 0;

        //this works for the minute
        public uint EstimatedTimeInMinutes { get; set; }

        public Complexity Complexity { get; set; }

        public Progress Progress { get; set; }
        
        //user assigned for the task might be null or the user itself since it's a standalone project
        public Guid? AssignedUserTo { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedTime { get; set; }

        public Issue? ParentIssue { get; set; }

        //public Guid ProjectId { get; set; }

        public Project? Project { get; set; }

        public ICollection<Comment>? Comments { get; set; }

        public ICollection<Note>? Notes { get; set; }

    }
}
