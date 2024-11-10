using System;
using System.Collections.Generic;

namespace Project_Manager.Models;

public partial class GroupUser
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? GroupId { get; set; }

    public Guid RoleId { get; set; }

    public virtual Group? Group { get; set; }

    public virtual AspNetUser? User { get; set; }
}
