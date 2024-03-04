using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class Block
{
    public int Uuid { get; set; }

    public int? DistrictId { get; set; }

    public int? BlockId { get; set; }

    public string BlockName { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual District? District { get; set; }

    public virtual ICollection<HalqaPanchayat> HalqaPanchayats { get; set; } = new List<HalqaPanchayat>();
}
