using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class Village
{
    public int Uuid { get; set; }

    public int? HalqaPanchayatId { get; set; }

    public int? TehsilId { get; set; }

    public string? VillageName { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual HalqaPanchayat? HalqaPanchayat { get; set; }

    public virtual Tehsil? Tehsil { get; set; }

    public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
}
