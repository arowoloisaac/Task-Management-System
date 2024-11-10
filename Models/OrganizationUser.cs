using System;
using System.Collections.Generic;

namespace Project_Manager.Models;

public partial class OrganizationUser
{
    public Guid Id { get; set; }

    public Guid? OrganizationId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? RoleId { get; set; }

    public virtual Organization? Organization { get; set; }

    public virtual AspNetRole? Role { get; set; }

    public virtual AspNetUser? User { get; set; }
}
