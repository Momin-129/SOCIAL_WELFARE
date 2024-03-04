using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class Document
{
    public int Uuid { get; set; }

    public int? CitizenId { get; set; }

    public string? Docs { get; set; }

    public DateTime? DateOfSubmission { get; set; }

    public virtual CitizenDetail? Citizen { get; set; }
}
