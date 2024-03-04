public class CitizenDetailsResultModel
{
    public int CitizenId { get; set; }
    public string? ApplicantName { get; set; }
    public string? ApplicantImage { get; set; }
    public DateTime Dob { get; set; }
    public string? Father_Guardian { get; set; }
    public string? FormSpecific { get; set; }
    public string? Category { get; set; }
    public int PresentAddressId { get; set; }
    public int PermanentAddressId { get; set; }
    public int BankDetailsId { get; set; }
    public string? MobileNumber { get; set; }
    public string? Email { get; set; }
    public int ServiceId { get; set; }
    public DateTime DateOfSubmission { get; set; }
    public string? ApplicationStatus { get; set; }

    // Present Address details
    public string? PresentAddress { get; set; }
    public string? PresentDistrict { get; set; }
    public string? PresentTehsil { get; set; }
    public string? PresentBlock { get; set; }
    public string? PresentPanchayatMuncipality { get; set; }
    public string? PresentVillage { get; set; }
    public string? PresentWard { get; set; }
    public int PresentPincode { get; set; }

    // Permanent Address details
    public string? PermanentAddress { get; set; }
    public string? PermanentDistrict { get; set; }
    public string? PermanentTehsil { get; set; }
    public string? PermanentBlock { get; set; }
    public string? PermanentPanchayatMuncipality { get; set; }
    public string? PermanentVillage { get; set; }
    public string? PermanentWard { get; set; }
    public int PermanentPincode { get; set; }

    // Bank details
    public string? BankName { get; set; }
    public string? BranchName { get; set; }
    public string? IFSCCode { get; set; }
    public string? AccountNumber { get; set; }

    // Documents
    public string? Documents { get; set; }
    public DateTime DocumentSubmissionDate { get; set; }

    // Request Phase details
    public string? Phases { get; set; }
    public string? Remarks { get; set; }
    public string? FormFields { get; set; }
}
