﻿namespace Project_Manager.Model
{
    public class Issue : ObjectDateTime
    {
        public Guid Id { get; set; }
        // has sub task bool+

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }

        public Guid UpdatedBy { get; set; }

        public Guid DeletedBy { get; set; }

        public Guid ChildIssueId { get; set; }

        public ICollection<Issue>? SubIssues { get; set; }
        //foreign key for the task for single projects
        //public Guid ProjectId { get; set; }

        public Project? Project { get; set; }

        public ICollection<Comment>? Comments { get; set; }

        public ICollection<Note>? Notes { get; set; }

    }
}
