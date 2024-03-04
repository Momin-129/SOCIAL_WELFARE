using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class BankDetail
{
    public int Uuid { get; set; }

    public string? BankName { get; set; }

    public string? BranchName { get; set; }

    public string? Ifsccode { get; set; }

    public string? AccountNumber { get; set; }

    public virtual ICollection<CitizenDetail> CitizenDetails { get; set; } = new List<CitizenDetail>();
}
