using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class Tag
{
    public int IdTag { get; set; }

    public string? NameTag { get; set; }

    public string? TypeTag { get; set; }

    public virtual ICollection<TagProduct> TagProducts { get; set; } = new List<TagProduct>();
}
