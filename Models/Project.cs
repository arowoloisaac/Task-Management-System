using System;
using System.Collections.Generic;

namespace Project_Manager.Models;

public partial class Project
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public int Progress { get; set; }

    public int Complexity { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? GroupId { get; set; }

    public Guid OrganizationId { get; set; }

    public DateTime CreatedTime { get; set; }

    public DateTime UpdatedTime { get; set; }

    public DateTime DeletedTime { get; set; }

    public DateTime ArchivedTime { get; set; }

    public virtual AspNetUser? Creator { get; set; }

    public virtual Group? Group { get; set; }

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public virtual Organization Organization { get; set; } = null!;

    public virtual ICollection<Wiki> Wikis { get; set; } = new List<Wiki>();
}
