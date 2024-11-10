﻿using System;
using System.Collections.Generic;

namespace Project_Manager.Models;

public partial class AspNetUserLogin
{
    public string LoginProvider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public string? ProviderDisplayName { get; set; }

    public Guid UserId { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
