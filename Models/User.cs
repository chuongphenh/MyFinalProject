using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string? NameAccount { get; set; }

    public string? NameUser { get; set; }

    public string? PasswordUser { get; set; }

    public bool? StatusUser { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual ICollection<UserPermision> UserPermisions { get; set; } = new List<UserPermision>();
}
