using System;
using System.Collections.Generic;

namespace ServicePlus.Models.Entities;

public partial class CitizenDetail
{
    public int Uuid { get; set; }

    public int UserId { get; set; }

    public string? ApplicantName { get; set; }

    public string? ApplicantImage { get; set; }

    public DateOnly? Dob { get; set; }

    public string? FatherGuardian { get; set; }

    public string? FormSpecific { get; set; }

    public string? Category { get; set; }

    public string? MobileNumber { get; set; }

    public string? Email { get; set; }

    public int? PresentAddressId { get; set; }

    public int? PermanentAddressId { get; set; }

    public int? BankDetailsId { get; set; }

    public int? ServiceId { get; set; }

    public DateTime? DateOfSubmission { get; set; }

    public string? ApplicationStatus { get; set; }

    public virtual BankDetail? BankDetails { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Address? PermanentAddress { get; set; }

    public virtual Address? PresentAddress { get; set; }

    public virtual ICollection<RequestPhase> RequestPhases { get; set; } = new List<RequestPhase>();

    public virtual Service? Service { get; set; }

    public virtual User User { get; set; } = null!;
}
