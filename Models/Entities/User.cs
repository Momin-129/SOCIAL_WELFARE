using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? UserTypeId { get; set; }

    public bool? Valid { get; set; }

    public virtual ICollection<CitizenDetail> CitizenDetails { get; set; } = new List<CitizenDetail>();
}
