using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class TagProduct
{
    public int Id { get; set; }

    public int? IdTag { get; set; }

    public int? IdProduct { get; set; }

    public virtual Product? IdProductNavigation { get; set; }

    public virtual Tag? IdTagNavigation { get; set; }
}
