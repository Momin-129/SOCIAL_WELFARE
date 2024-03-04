using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class Address
{
    public int AddressId { get; set; }

    public int? DistrictId { get; set; }

    public int? TehsilId { get; set; }

    public int? BlockId { get; set; }

    public int? HalqaPanchayatId { get; set; }

    public int? VillageId { get; set; }

    public int? WardId { get; set; }

    public int? PincodeId { get; set; }

    public string? AddressDetails { get; set; }

    public virtual Block? Block { get; set; }

    public virtual ICollection<CitizenDetail> CitizenDetailPermanentAddresses { get; set; } = new List<CitizenDetail>();

    public virtual ICollection<CitizenDetail> CitizenDetailPresentAddresses { get; set; } = new List<CitizenDetail>();

    public virtual District? District { get; set; }

    public virtual HalqaPanchayat? HalqaPanchayat { get; set; }

    public virtual Pincode? Pincode { get; set; }

    public virtual Tehsil? Tehsil { get; set; }

    public virtual Village? Village { get; set; }

    public virtual Ward? Ward { get; set; }
}
