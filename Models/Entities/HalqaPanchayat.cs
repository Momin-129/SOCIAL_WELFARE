using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class HalqaPanchayat
{
    public int Uuid { get; set; }

    public int? BlockId { get; set; }

    public string? PanchayatName { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Block? Block { get; set; }

    public virtual ICollection<Village> Villages { get; set; } = new List<Village>();
}
