using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class Ward
{
    public int Uuid { get; set; }

    public int? VillageId { get; set; }

    public string? WardName { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Village? Village { get; set; }
}
