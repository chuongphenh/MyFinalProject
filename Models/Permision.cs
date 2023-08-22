using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class Permision
{
    public string IdPer { get; set; } = null!;

    public string? GroupPer { get; set; }

    public string? NamePer { get; set; }

    public virtual ICollection<UserPermision> UserPermisions { get; set; } = new List<UserPermision>();
}
