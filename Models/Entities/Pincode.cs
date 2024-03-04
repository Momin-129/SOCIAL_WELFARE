using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class Pincode
{
    public int PincodeId { get; set; }

    public int? PincodeNumber { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
