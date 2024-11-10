using System;
using System.Collections.Generic;

namespace Project_Manager.Models;

public partial class Issue
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int IssueType { get; set; }

    public int IssueLevel { get; set; }

    public long TimeSpent { get; set; }

    public long EstimatedTimeInMinutes { get; set; }

    public int Complexity { get; set; }

    public int Progress { get; set; }

    public Guid? AssignedUserTo { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedTime { get; set; }

    public Guid? ParentIssueId { get; set; }

    public Guid? ProjectId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Issue> InverseParentIssue { get; set; } = new List<Issue>();

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual Issue? ParentIssue { get; set; }

    public virtual Project? Project { get; set; }
}
