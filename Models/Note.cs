using System;
using System.Collections.Generic;

namespace Project_Manager.Models;

public partial class Note
{
    public Guid Id { get; set; }

    public string Description { get; set; } = null!;

    public Guid IssueId { get; set; }

    public Guid? UsersId { get; set; }

    public virtual Issue Issue { get; set; } = null!;

    public virtual AspNetUser? Users { get; set; }
}
