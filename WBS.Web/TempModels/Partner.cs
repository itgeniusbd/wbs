using System;
using System.Collections.Generic;

namespace WBS.Web.TempModels;

public partial class Partner
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? NameBn { get; set; }

    public string? LogoUrl { get; set; }

    public string? Website { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }
}
