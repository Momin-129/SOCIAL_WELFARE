using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class Otpstore
{
    public string? Otp { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
