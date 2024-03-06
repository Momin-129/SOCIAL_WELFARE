using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServicePlus.Models.Entities;
using Newtonsoft.Json;
public class HelperFunction
{
    private readonly ServicePlusContext dbContext;
    private readonly ILogger<HelperFunction> _logger;

    private readonly IWebHostEnvironment _webHostEnvironment;
    public HelperFunction(ServicePlusContext dbContext, ILogger<HelperFunction> logger, IWebHostEnvironment webHostEnvironment)
    {
        this.dbContext = dbContext;
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<string> GetFilePath(IFormFile? docFile)
    {
        string docPath = "";
        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        string uniqueName = Guid.NewGuid().ToString() + "_" + docFile?.FileName;

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        if (docFile != null && docFile.Length > 0)
        {
            string filePath = Path.Combine(uploadsFolder, uniqueName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await docFile.CopyToAsync(stream);
            }

            docPath = "~/uploads/" + uniqueName;
        }

        return docPath;
    }

    public (int preAddressId, int perAddressId) GetAddressId(IFormCollection form)
    {
        string? preAddress = form["PresentAddress"];
        int.TryParse(form["PresentDistrict"], out int preDistrictIdValue);
        int.TryParse(form["PresentTehsil"], out int preTehsilIdValue);
        int.TryParse(form["PresentBlock"], out int preBlockIdValue);
        string? prePanchayat = form["PresentPanchayatMuncipality"];
        string? preVillage = form["PresentVillage"];
        string? preWard = form["PresentWard"];
        string? prePincode = form["PresentPincode"];

        bool isCheckboxChecked = form["SameAsPresent"] == "oN";

        string? perAddress = form["PermanentAddress"];
        int.TryParse(form["PermanentDistrict"], out int perDistrictIdValue);
        int.TryParse(form["PermanentTehsil"], out int perTehsilIdValue);
        int.TryParse(form["PermanentBlock"], out int perBlockIdValue);
        string? perPanchayat = form["PermanentPanchayatMuncipality"];
        string? perVillage = form["PermanentVillage"];
        string? perWard = form["PermanentWard"];
        string? perPincode = form["PermanentPincode"];

        var districtIdParam = new SqlParameter("@DistrictId", preDistrictIdValue);
        var tehsilIdParam = new SqlParameter("@TehsilId", preTehsilIdValue);
        var blockIdParam = new SqlParameter("@BlockId", preBlockIdValue);
        var halqaPanchayatNameParam = new SqlParameter("@HalqaPanchayatName", prePanchayat ?? (object)DBNull.Value);
        var villageNameParam = new SqlParameter("@VillageName", preVillage ?? (object)DBNull.Value);
        var wardNameParam = new SqlParameter("@WardName", preWard ?? (object)DBNull.Value);
        var pincodeParam = new SqlParameter("@Pincode", prePincode ?? (object)DBNull.Value);
        var addressDetailsParam = new SqlParameter("@AddressDetails", preAddress ?? (object)DBNull.Value);

        var perDistrictIdParam = new SqlParameter("@DistrictId", perDistrictIdValue);
        var perTehsilIdParam = new SqlParameter("@TehsilId", perTehsilIdValue);
        var perBlockIdParam = new SqlParameter("@BlockId", perBlockIdValue);
        var perHalqaPanchayatNameParam = new SqlParameter("@HalqaPanchayatName", perPanchayat ?? (object)DBNull.Value);
        var perVillageNameParam = new SqlParameter("@VillageName", perVillage ?? (object)DBNull.Value);
        var perWardNameParam = new SqlParameter("@WardName", perWard ?? (object)DBNull.Value);
        var perPincodeParam = new SqlParameter("@Pincode", perPincode ?? (object)DBNull.Value);
        var perAddressDetailsParam = new SqlParameter("@AddressDetails", perAddress ?? (object)DBNull.Value);


        var preAddressId = dbContext.Address.FromSqlRaw("EXEC CheckAndInsertAddress  @DistrictId ,@TehsilId ,@BlockId ,@HalqaPanchayatName ,@VillageName ,@WardName ,@Pincode ,@AddressDetails ", districtIdParam,
        tehsilIdParam,
        blockIdParam,
        halqaPanchayatNameParam,
        villageNameParam,
        wardNameParam,
        pincodeParam,
        addressDetailsParam).ToList();

        int perAddressId;

        if (isCheckboxChecked)
        {
            perAddressId = preAddressId[0].AddressId;
        }
        else
        {
            var result = dbContext.Address.FromSqlRaw("EXEC CheckAndInsertAddress  @DistrictId ,@TehsilId ,@BlockId ,@HalqaPanchayatName ,@VillageName ,@WardName ,@Pincode ,@AddressDetails ", perDistrictIdParam,
            perTehsilIdParam,
            perBlockIdParam,
            perHalqaPanchayatNameParam,
            perVillageNameParam,
            perWardNameParam,
            perPincodeParam,
            perAddressDetailsParam).ToList();

            perAddressId = result[0].AddressId;
        }



        return (preAddressId[0].AddressId, perAddressId);

    }

    public int GetBankId(IFormCollection form)
    {

        string? bankName = form["BankName"];
        string? branchName = form["BranchName"];
        string? ifcsCode = form["IfscCode"];
        string? accNumber = form["AccountNumber"];

        SqlParameter bankNameParam = new SqlParameter("@BankName", bankName ?? (object)DBNull.Value);
        SqlParameter branchNameParam = new SqlParameter("@BranchName", branchName ?? (object)DBNull.Value);
        SqlParameter ifcsCodeParam = new SqlParameter("@IFSCCode", ifcsCode ?? (object)DBNull.Value);
        SqlParameter accNumberParam = new SqlParameter("@AccountNumber", accNumber ?? (object)DBNull.Value);

        var result = dbContext.BankDetails.FromSqlRaw("EXEC InsertBankDetails @BankName , @BranchName ,@IFSCCode , @AccountNumber ", bankNameParam, branchNameParam, ifcsCodeParam, accNumberParam).ToList();

        return result[0].Uuid;
    }


    public async Task<int> InsertCitizenDetails(IFormCollection form, int preAddressId, int perAddressId, int BankDetailsId, int serviceId, int UserId)
    {
        IFormFile? photographFile = form.Files["ApplicantImage"];


        string? name = form["ApplicantName"];
        string? dob = form["DOB"];
        string? guardian = form["Father_Guardian"];
        string? category = form["Category"];
        string? number = form["MobileNumber"];
        string? email = form["Email"];


        string photographPath = await GetFilePath(photographFile);
        string? FormSpecific = "";
        if (serviceId == 32)
        {
            int.TryParse(form["District"], out int districtId);
            int.TryParse(form["Teshil"], out int tehsilId);
            string? pensionType = form["pensionType"];
            string? gender = form["gender"];


            FormSpecific = JsonConvert.SerializeObject(new
            {
                District = districtId,
                Tehsil = tehsilId,
                PensionType = pensionType,
                Gender = gender
            });

        }
        else if (serviceId == 40)
        {
            int.TryParse(form["District"], out int districtId);
            string? motherName = form["MotherName"];
            string? dateOfMarriage = form["DateOfMarriage"];
            FormSpecific = JsonConvert.SerializeObject(new
            {
                District = districtId,
                MotherName = motherName,
                DateOfMarriage = dateOfMarriage
            });

        };

        _logger.LogInformation("Service Id {0}", serviceId);

        var userIdParam = new SqlParameter("@UserId", UserId);
        var applicantNameParam = new SqlParameter("@ApplicantName", name ?? (object)DBNull.Value);
        var applicantImageParam = new SqlParameter("@ApplicantImage", photographPath);
        var dobParam = new SqlParameter("@DOB", dob ?? (object)DBNull.Value);
        var parentageParam = new SqlParameter("@Parentage", guardian ?? (object)DBNull.Value);
        var categoryParam = new SqlParameter("@Category", category ?? (object)DBNull.Value);
        var mobileNumberParam = new SqlParameter("@MobileNumber", number);
        var mailIdParam = new SqlParameter("@Email", email ?? (object)DBNull.Value);
        var presentAddressIdParam = new SqlParameter("@PresentAddressId", preAddressId);
        var permanentAddressIdParam = new SqlParameter("@PermanentAddressId", perAddressId);
        var bankDetailsIdParam = new SqlParameter("@BankDetailsId", BankDetailsId);
        var serviceIdParam = new SqlParameter("@ServiceId", serviceId);
        var formSpecificParam = new SqlParameter("@FormSpecific", FormSpecific);


        var result = dbContext.CitizenDetails.FromSqlRaw("EXEC InsertCitizenDetails @UserId,@ApplicantName, @ApplicantImage, @DOB, @Parentage, @FormSpecific, @Category, @MobileNumber, @Email, @PresentAddressId, @PermanentAddressId, @BankDetailsId, @ServiceId",
        userIdParam,
        applicantNameParam,
        applicantImageParam,
        dobParam,
        parentageParam,
        formSpecificParam,
        categoryParam,
        mobileNumberParam,
        mailIdParam,
        presentAddressIdParam,
        permanentAddressIdParam,
        bankDetailsIdParam,
        serviceIdParam).ToList();

        return result[0].Uuid;
    }

    public void InsertParmeter(ref List<SqlParameter> parameter, string parameterName, string parameterValue, ref string query)
    {
        if (int.TryParse(parameterValue, out int intValue))
        {
            parameter.Add(new SqlParameter(parameterName, intValue));
        }
        else
        {
            parameter.Add(new SqlParameter(parameterName, parameterValue));
        }
        query += parameterName + "=" + parameterName + ",";
    }

}