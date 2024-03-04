using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class ServiceSpecific
{
    public int Uuid { get; set; }

    public int? ServiceId { get; set; }

    public string? Phases { get; set; }

    public string? CitizenDocuments { get; set; }

    public string? OfficerDocuments { get; set; }

    public string? FormElements { get; set; }

    public virtual Service? Service { get; set; }
}
