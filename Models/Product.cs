using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public string? NameProduct { get; set; }

    public string? DescriptionProduct { get; set; }

    public string? ContentProduct { get; set; }

    public string? PhotoProduct { get; set; }

    public int? HotProduct { get; set; }

    public double? PriceProduct { get; set; }

    public double? DiscountProduct { get; set; }

    public virtual ICollection<CategoryProduct> CategoryProducts { get; set; } = new List<CategoryProduct>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<TagProduct> TagProducts { get; set; } = new List<TagProduct>();
}
