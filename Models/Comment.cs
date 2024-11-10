using System;
using System.Collections.Generic;

namespace Project_Manager.Models;

public partial class Comment
{
    public Guid Id { get; set; }

    public string Descriptiom { get; set; } = null!;

    public Guid? IssueId { get; set; }

    public Guid? UserId { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid UpdatedBy { get; set; }

    public Guid DeletedBy { get; set; }

    public virtual Issue? Issue { get; set; }

    public virtual AspNetUser? User { get; set; }
}
