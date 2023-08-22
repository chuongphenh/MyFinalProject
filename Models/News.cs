using System;
using System.Collections.Generic;

namespace MyFinalProject.Models;

public partial class News
{
    public int IdNews { get; set; }

    public string? NameNews { get; set; }

    public string? DescriptionNews { get; set; }

    public string? ContentNews { get; set; }

    public int? HotNews { get; set; }

    public string? PhotoNews { get; set; }
}
