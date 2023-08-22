using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class Category
{
    public int IdCategory { get; set; }

    public int? IdParent { get; set; }

    public string? NameCategory { get; set; }

    public int? DisplayHomePage { get; set; }

    public string? IconCategory { get; set; }

    public virtual ICollection<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();
}
