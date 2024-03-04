using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class RequestPhase
{
    public int Uuid { get; set; }

    public int? CitizenId { get; set; }

    public string? Phases { get; set; }

    public string? Remarks { get; set; }

    public string? FormFields { get; set; }

    public virtual CitizenDetail? Citizen { get; set; }
}
