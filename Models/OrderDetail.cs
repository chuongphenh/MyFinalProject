using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int? IdOrder { get; set; }

    public int? IdProduct { get; set; }

    public int? Quantity { get; set; }

    public double? Price { get; set; }

    public virtual Order? IdOrderNavigation { get; set; }

    public virtual Product? IdProductNavigation { get; set; }
}
