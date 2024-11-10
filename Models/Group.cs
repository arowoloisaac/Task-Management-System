using System;
using System.Collections.Generic;

namespace Project_Manager.Models;

public partial class Group
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public Guid OrganizationId { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public Guid DeletedBy { get; set; }

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();

    public virtual ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();

    public virtual Organization Organization { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
