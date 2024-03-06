using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServicePlus.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;





namespace ServicePlus.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly ServicePlusContext dbContext;
        private readonly ILogger<UserController> _logger;

        private readonly HelperFunction _helper;

        public UserController(ServicePlusContext dbContext, ILogger<UserController> logger, HelperFunction helper)
        {
            this.dbContext = dbContext;
            _logger = logger;
            _helper = helper;
        }

        public IActionResult Index()
        {
            ViewData["UserType"] = "Citizen";
            return View();
        }

        public async Task<IActionResult> ServiceList()
        {
            ViewData["UserType"] = "Citizen";

            var services = await dbContext.Services.ToListAsync();
            return View(services);
        }



        [HttpPost]
        public IActionResult SetViewData(string serviceName, string serviceId)
        {

            Response.Cookies.Append("formType", serviceName, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(1) // Set an appropriate expiration time
            });

            Response.Cookies.Append("serviceId", serviceId, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(1) // Set an appropriate expiration time
            });


            return Ok();
        }

        [HttpGet]
        public IActionResult Form()
        {
            ViewData["UserType"] = "Citizen";

            if (Request.Cookies.TryGetValue("formType", out var serviceName))
            {
                ViewData["formType"] = serviceName;
            }
            if (Request.Cookies.TryGetValue("serviceId", out var serviceId))
            {
                ViewData["serviceId"] = serviceId;
            }

            var serviceIdParam = new SqlParameter("@ServiceId", serviceId);
            var result = dbContext.ServiceSpecifics.FromSqlRaw("EXEC GetServiceSpecific @ServiceId", serviceIdParam).ToList()[0];


            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Form([FromForm] IFormCollection form)
        {

            var result = _helper.GetAddressId(form);
            int preAddressId = result.preAddressId;
            int perAddressId = result.perAddressId;
            int BankDetailsId = _helper.GetBankId(form);

            if (Request.Cookies.TryGetValue("serviceId", out var serviceId))
            {
                ViewData["serviceId"] = serviceId;
            }

            string userId = HttpContext.Session.GetString("UserId")!;


            int.TryParse(serviceId, out int ServiceId);
            int.TryParse(userId, out int UserId);

            int citizenId = await _helper.InsertCitizenDetails(form, preAddressId, perAddressId, BankDetailsId, ServiceId, UserId);


            // Document Insert Logic
            List<Dictionary<string, object>> dataArray = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(form["docs"]!)!;

            var objectsList = new List<object>();
            // Process the dataArray here
            foreach (var element in dataArray)
            {
                // Your iteration logic
                foreach (var key in element.Keys)
                {
                    if (key.Equals("label", StringComparison.OrdinalIgnoreCase))
                    {
                        string labelKey = element[key].ToString()!;
                        string id = labelKey.Replace(" ", "");
                        string enclosure = form["enclosure" + id]!;

                        IFormFile fileId = form.Files["file" + id]!;
                        string filePath = await _helper.GetFilePath(fileId);

                        objectsList.Add(new { label = labelKey, Enclosure = enclosure, FileReference = filePath });
                    }
                }
            }

            var jsonString = JsonConvert.SerializeObject(objectsList);

            var citizenIdParam = new SqlParameter("@CitizenId", citizenId);
            var docsParam = new SqlParameter("@Docs", jsonString);

            dbContext.Documents.FromSqlRaw("EXEC InsertDocuments @CitizenId, @Docs", citizenIdParam, docsParam).ToList();

            return RedirectToAction("Index");
        }



        public IActionResult ApplicationStatus()
        {
            ViewData["UserType"] = "Citizen";
            string userId = HttpContext.Session.GetString("UserId")!;
            int.TryParse(userId, out int UserId);


            var userIdParam = new SqlParameter("@UserId", UserId);

            var applications = dbContext.CitizenDetails.FromSqlRaw("EXEC GetApplicationsStatus @UserId", userIdParam).ToList();
            return View(applications);
        }


        public IActionResult IncompleteApplications()
        {
            ViewData["UserType"] = "Citizen";
            string userId = HttpContext.Session.GetString("UserId")!;
            int.TryParse(userId, out int UserId);


            var userIdParam = new SqlParameter("@UserId", UserId);

            var applications = dbContext.CitizenDetails.FromSqlRaw("EXEC GetIncompleteApplications @UserId", userIdParam).ToList();
            return View(applications);
        }

        public IActionResult EditForm(string citizenId, string phase)
        {
            ViewData["UserType"] = "Citizen";
            ViewData["Phase"] = phase;
            int.TryParse(citizenId, out int CitizenId);
            var citizenIdParam = new SqlParameter("@CitizenId", CitizenId);
            var citizenDetails = dbContext.CitizenDetailsResultModels.FromSqlRaw("EXEC GetCitizenDetailsByUuid @CitizenId", citizenIdParam).AsNoTracking().ToList();

            return View(citizenDetails);
        }

        [HttpPost]
        public async Task<IActionResult> EditForm([FromForm] IFormCollection form)
        {
            string CitizenTableQuery = "EXEC UpdateCitizenDetails ";
            string preAddressQuery = "EXEC UpdateAddress ";
            string perAddressQuery = "EXEC UpdateAddress ";
            string bankDetailQuery = "EXEC UpdateBankDetails ";

            List<SqlParameter> citizenParameters = [];
            List<SqlParameter> preAddressParameters = [];
            List<SqlParameter> perAddressParameters = [];
            List<SqlParameter> bankDetailParameters = [];

            foreach (var key in form.Keys)
            {
                if (key != "citizenId" && key != "presentAddressId" && key != "permanentAddressId" && key != "bankDetailsId" && key != "currentPhase")
                {

                    if (key == "ApplicantName" || key == "DOB" || key == "Father_Guardian" || key == "FormSpecific" || key == "Category" || key == "MobileNumber" || key == "Email")
                    {
                        var parameterName = "@" + key;
                        var parameterValue = form[key.ToString()].ToString();
                        _helper.InsertParmeter(ref citizenParameters, parameterName, parameterValue, ref CitizenTableQuery);

                    }
                    if (key == "PresentAddress" || key == "PresentDistrict" || key == "PresentTehsil" || key == "PresentBlock" || key == "PresentPanchayatMuncipality" || key == "PresentVillage" || key == "PresentWard" || key == "PresentPincode")
                    {
                        var parameterName = "@" + key.Replace("Present", ""); ;
                        var parameterValue = form[key.ToString()].ToString();
                        _helper.InsertParmeter(ref preAddressParameters, parameterName, parameterValue, ref preAddressQuery);
                    }
                    if (key == "PermanentAddress" || key == "PermanentDistrict" || key == "PermanentTehsil" || key == "PermanentBlock" || key == "PermanentPanchayatMuncipality" || key == "PermanentVillage" || key == "PermanentWard" || key == "PermanentPincode")
                    {
                        var parameterName = "@" + key.Replace("Permanent", "");
                        var parameterValue = form[key.ToString()].ToString();
                        _helper.InsertParmeter(ref perAddressParameters, parameterName, parameterValue, ref perAddressQuery);
                    }
                    if (key == "BankName" || key == "BranchName" || key == "IfscCode" || key == "AccountNumber")
                    {
                        var parameterName = "@" + key;
                        var parameterValue = form[key.ToString()].ToString();
                        _helper.InsertParmeter(ref bankDetailParameters, parameterName, parameterValue, ref bankDetailQuery);
                    }

                }
            }


            foreach (var file in form.Files)
            {
                if (file.Name == "ApplicantImage")
                {
                    string path = await _helper.GetFilePath(file);
                    var parameterName = "@ApplicantImage";
                    citizenParameters.Add(new SqlParameter(parameterName, path));
                    CitizenTableQuery += parameterName + "=" + parameterName + ",";
                    _logger.LogInformation($"FILE KEY: {file.Name}, FILENAME: {file.FileName}");
                }
            }




            if (preAddressParameters.Count > 0)
            {
                preAddressQuery = preAddressQuery.Remove(preAddressQuery.Length - 1);
                int.TryParse(form["presentAddressId"], out int preAddressId);
                preAddressParameters.Add(new SqlParameter("@AddressId", preAddressId));
                preAddressQuery += ",@AddressId=@AddressId";
                dbContext.Database.ExecuteSqlRaw(preAddressQuery, preAddressParameters.ToArray());

            }

            if (perAddressParameters.Count > 0)
            {
                perAddressQuery = perAddressQuery.Remove(perAddressQuery.Length - 1);
                int.TryParse(form["permanentAddressId"], out int perAddressId);
                perAddressParameters.Add(new SqlParameter("@AddressId", perAddressId));
                perAddressQuery += ",@AddressId=@AddressId";
                dbContext.Database.ExecuteSqlRaw(perAddressQuery, perAddressParameters.ToArray());
            }

            if (bankDetailParameters.Count > 0)
            {
                bankDetailQuery = bankDetailQuery.Remove(bankDetailQuery.Length - 1);
                int.TryParse(form["bankDetailsId"], out int bankDetailsId);
                bankDetailParameters.Add(new SqlParameter("@BankId", bankDetailsId));
                bankDetailQuery += ",@BankId=@BankId";
                dbContext.Database.ExecuteSqlRaw(bankDetailQuery, bankDetailParameters.ToArray());
            }


            if (citizenParameters.Count > 0)
            {
                CitizenTableQuery = CitizenTableQuery.TrimEnd(',');
                int.TryParse(form["citizenId"], out int citizenId);
                string? currentPhase = form["currentPhase"];
                citizenParameters.Add(new SqlParameter("@CitizenId", citizenId));
                citizenParameters.Add(new SqlParameter("@CurrentPhase", currentPhase));

                CitizenTableQuery += ",@CitizenId=@CitizenId,@CurrentPhase=@CurrentPhase";
                dbContext.Database.ExecuteSqlRaw(CitizenTableQuery, citizenParameters.ToArray());
            }

            _logger.LogInformation($"Citizen Query : {CitizenTableQuery}");

            return Json(new { success = true, url = "/User/Index" });

        }

        [HttpGet]
        public async Task<IActionResult> GetDistricts()
        {
            var districts = await dbContext.Districts.ToListAsync();

            return Json(new { success = true, districts });
        }

        [HttpPost]
        public IActionResult GetTehsilsByDistrict(string districtName)
        {
            var tehsils = dbContext.Tehsils.FromSqlRaw(
            "EXEC GetTehsilsByDistrict @DistrictName",
            new SqlParameter("@DistrictName", districtName)
        ).ToList();

            // Return the result as a partial view
            return Json(new { success = true, tehsils });
        }

        [HttpGet]
        public IActionResult GetServiceName(int Id)
        {
            var Service = dbContext.Services.FirstOrDefault(u => u.ServiceId == Id);
            return Json(new { success = true, Service });
        }

        [HttpGet]
        public IActionResult GetApplicationDetails(int citizenId)
        {
            var CitizenIdParam = new SqlParameter("@CitizenId", citizenId);
            var result = dbContext.RequestPhases.FromSqlRaw("EXEC GetApplicationDetails @CitizenId", CitizenIdParam).AsNoTracking().ToList();
            return Json(new { success = true, result });
        }


        public IActionResult GetBlocksByDistrict(string districtName)
        {
            var blocks = dbContext.Blocks.FromSqlRaw(
            "EXEC GetBlocksByDistrict @DistrictName",
            new SqlParameter("@DistrictName", districtName)
        ).ToList();

            // Return the result as a partial view
            return Json(new { success = true, blocks });
        }



    }
}