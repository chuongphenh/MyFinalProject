using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class Rating
{
    public int IdRating { get; set; }

    public int? IdProduct { get; set; }

    public int? Star { get; set; }

    public virtual Product? IdProductNavigation { get; set; }
}
