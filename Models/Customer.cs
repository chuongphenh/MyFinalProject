using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class Customer
{
    public int IdCustomer { get; set; }

    public string? NameCustomer { get; set; }

    public string? EmailCustomer { get; set; }

    public string? AddressCustomer { get; set; }

    public string? PhoneCustomer { get; set; }

    public string? PasswordCustomer { get; set; }

    public bool? StatusUser { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
