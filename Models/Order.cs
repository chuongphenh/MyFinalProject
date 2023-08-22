using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class Order
{
    public int IdOrder { get; set; }

    public int? IdCustomer { get; set; }

    public double? PriceOrder { get; set; }

    public int? StatusOrder { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual Customer? IdCustomerNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
