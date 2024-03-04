using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public string? DepartmentName { get; set; }

    public string? State { get; set; }

    public virtual ICollection<CitizenDetail> CitizenDetails { get; set; } = new List<CitizenDetail>();

    public virtual ICollection<ServiceSpecific> ServiceSpecifics { get; set; } = new List<ServiceSpecific>();
}
