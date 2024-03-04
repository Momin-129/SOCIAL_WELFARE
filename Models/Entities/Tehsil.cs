using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class Tehsil
{
    public int Uuid { get; set; }

    public int? DistrictId { get; set; }

    public int? TehsilId { get; set; }

    public string TehsilName { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual District? District { get; set; }

    public virtual ICollection<Village> Villages { get; set; } = new List<Village>();
}
