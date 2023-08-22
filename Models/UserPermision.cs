using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class UserPermision
{
    public int IdUser { get; set; }

    public string IdPer { get; set; } = null!;

    public string? Note { get; set; }

    public virtual Permision IdPerNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
