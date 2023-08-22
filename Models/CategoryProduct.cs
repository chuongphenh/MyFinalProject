using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class CategoryProduct
{
    public int Id { get; set; }

    public int? IdCategory { get; set; }

    public int? IdProduct { get; set; }

    public virtual Category? IdCategoryNavigation { get; set; }

    public virtual Product? IdProductNavigation { get; set; }
}
