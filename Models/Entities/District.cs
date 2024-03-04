using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class District
{
    public int Uuid { get; set; }

    public int? DistrictId { get; set; }

    public string DistrictName { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Block> Blocks { get; set; } = new List<Block>();

    public virtual ICollection<Tehsil> Tehsils { get; set; } = new List<Tehsil>();
}
