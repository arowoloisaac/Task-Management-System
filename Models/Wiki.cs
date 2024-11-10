using System;
using System.Collections.Generic;

namespace Project_Manager.Models;

public partial class Wiki
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedTime { get; set; }

    public DateTime UpdatedTime { get; set; }

    public DateTime DeletedTime { get; set; }

    public Guid? ProjectId { get; set; }

    public Guid? UserId { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public Guid DeletedBy { get; set; }

    public virtual Project? Project { get; set; }

    public virtual AspNetUser? User { get; set; }
}
