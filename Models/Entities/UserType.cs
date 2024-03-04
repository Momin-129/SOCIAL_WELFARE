using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class UserType
{
    public int UserTypeId { get; set; }

    public string UserTypeValue { get; set; } = null!;
}
