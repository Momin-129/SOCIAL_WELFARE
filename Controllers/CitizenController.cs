using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SendEmails;
using ServicePlus.Models.Entities;

namespace ServicePlus.Controllers
{
    public class CitizenController : Controller
    {
        private readonly ServicePlusContext dbContext;
        private readonly ILogger<CitizenController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly PdfFillService _pdfFileService;

        public CitizenController(ServicePlusContext dbContext, ILogger<CitizenController> logger, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment, PdfFillService pdfFillService)
        {
            this.dbContext = dbContext;
            _logger = logger;
            this._emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
            _pdfFileService = pdfFillService;
        }


        public IActionResult CitizenDetail(string id)
        {
            var value = HttpContext.Session.GetString("UserType");
            ViewData["UserType"] = "Officer";

            int.TryParse(id, out int citizenId);
            var CitizenIdParam = new SqlParameter("@CitizenId", citizenId);


            var result = dbContext.CitizenDetailsResultModels.FromSqlRaw("EXEC GetCitizenDetailsByUuid @CitizenId", CitizenIdParam).AsNoTracking().ToList();


            return View(result);
        }


        [HttpPost]
        public IActionResult ForwardAction([FromForm] IFormCollection form)
        {

            var value = HttpContext.Session.GetString("UserType");
            ViewData["UserType"] = value;

            int.TryParse(form["citizenId"], out int citizenId);
            string? currentPhase = form["currentPhase"];
            string? nextPhase = form["nextPhase"];
            string? remarks = form["remarks"];

            var citizenIdParam = new SqlParameter("@CitizenId", citizenId);
            var currentPhaseParam = new SqlParameter("@CurrentPhase", currentPhase);
            var nextPhaseParam = new SqlParameter("@NextPhase", nextPhase);
            var remarksParam = new SqlParameter("@Remarks", remarks);

            _logger.LogInformation($"Current Phase: {currentPhase} Next Phase: {nextPhase}");

            dbContext.Database.ExecuteSqlRaw("EXEC ForwardRequest @CurrentPhase, @NextPhase, @CitizenId, @Remarks", currentPhaseParam, nextPhaseParam, citizenIdParam, remarksParam);

            string? url = "";

            if (value != "User")
            {
                url = "/Officer/Index";
            }
            else
            {
                url = "/User/Index";
            }

            return Json(new { status = true, url = url });
        }

        [HttpPost]
        public IActionResult RejectAction([FromForm] IFormCollection form)
        {
            var value = HttpContext.Session.GetString("UserType");
            ViewData["UserType"] = value;

            int.TryParse(form["citizenId"], out int citizenId);
            string? currentPhase = form["currentPhase"];
            string? remarks = form["remarks"];


            var citizenIdParam = new SqlParameter("@CitizenId", citizenId);
            var currentPhaseParam = new SqlParameter("@CurrentPhase", currentPhase);
            var remarksParam = new SqlParameter("@Remarks", remarks);

            dbContext.Database.ExecuteSqlRaw("EXEC RejectRequest @CurrentPhase, @CitizenId, @Remarks", currentPhaseParam, citizenIdParam, remarksParam);

            string? url = "";

            if (value != "User")
            {
                url = "/Officer/Index";
            }
            else
            {
                url = "/User/Index";
            }

            return Json(new { status = true, url = url });
        }

        [HttpPost]
        public IActionResult ReturnAction([FromForm] IFormCollection form)
        {
            var value = HttpContext.Session.GetString("UserType");
            ViewData["UserType"] = value;

            int.TryParse(form["citizenId"], out int citizenId);
            string? currentPhase = form["currentPhase"];
            string? remarks = form["remarks"];
            var returnObject = form["returnObject"].ToString();


            var citizenIdParam = new SqlParameter("@CitizenId", citizenId);
            var currentPhaseParam = new SqlParameter("@CurrentPhase", currentPhase);
            var remarksParam = new SqlParameter("@Remarks", remarks);
            var returnObjectParam = new SqlParameter("@ReturnObject", returnObject);

            dbContext.Database.ExecuteSqlRaw("EXEC ReturnRequest @CurrentPhase, @CitizenId, @Remarks,@ReturnObject", currentPhaseParam, citizenIdParam, remarksParam, returnObjectParam);

            string? url = "";

            if (value != "User")
            {
                url = "/Officer/Index";
            }
            else
            {
                url = "/User/Index";
            }

            return Json(new { status = true, url = url });
        }


        [HttpPost]
        public async Task<IActionResult> SanctionAction([FromForm] IFormCollection form)
        {

            JObject citizenDetails = JsonConvert.DeserializeObject<JObject>(form["citizenDetails"]!)!;

            var value = HttpContext.Session.GetString("UserType");
            ViewData["UserType"] = value;

            JObject FormSpecific = JsonConvert.DeserializeObject<JObject>(citizenDetails["formSpecific"]!.ToString())!;




            int.TryParse(form["citizenId"], out int citizenId);
            string? currentPhase = form["currentPhase"];
            string? remarks = form["remarks"];



            var citizenIdParam = new SqlParameter("@CitizenId", citizenId);
            var currentPhaseParam = new SqlParameter("@CurrentPhase", currentPhase);
            var remarksParam = new SqlParameter("@Remarks", remarks);


            dbContext.Database.ExecuteSqlRaw("EXEC SanctionRequest @CurrentPhase, @CitizenId, @Remarks", currentPhaseParam, citizenIdParam, remarksParam);

            var to = citizenDetails["email"]!.ToString().ToLower();
            var subject = "Test Email";
            var body = "This is a test email body.";

            _logger.LogInformation($"EMAIL {citizenDetails["email"]}");


            string inputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "resources", "MarriageSanction.pdf");

            string outputFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "resources", citizenId + "SanctionLetter.pdf");


            var fieldValues = new Dictionary<string, string>
            {
                {"name", citizenDetails["applicantName"]!.ToString()},
                {"dob", "23-07-2000"},
                {"father_guardian", citizenDetails["father_Guardian"]!.ToString()},
                {"motherName", FormSpecific["MotherName"]!.ToString()},
                {"mobile_email", citizenDetails["mobileNumber"]!.ToString()+"/"+citizenDetails["email"]!.ToString()},
                {"dateOfMarraige", FormSpecific["DateOfMarriage"]!.ToString()},
                {"bankName_branchName",  citizenDetails["bankName"]!.ToString()},
                {"ifsc_acc",  citizenDetails["ifscCode"]!.ToString() + "/" + citizenDetails["accountNumber"]!.ToString()},
                {"amount_sanctioned", "50000(Fifty thousand only)"},
                {"preAddress", citizenDetails["presentAddress"]!.ToString() +", TEHSIL:"+ citizenDetails["presentTehsil"]!.ToString() +", \nDISTRICT:"+ citizenDetails["presentDistrict"]!.ToString() +", PINCODE:"+ citizenDetails["presentPincode"]!.ToString()  },
               {"perAddress", citizenDetails["permanentAddress"]!.ToString() +", TEHSIL:"+ citizenDetails["permanentTehsil"]!.ToString() +", \nDISTRICT:"+ citizenDetails["permanentDistrict"]!.ToString() +", PINCODE:"+ citizenDetails["permanentPincode"]!.ToString()  },

            };



            _pdfFileService.FillPdf(inputFilePath, outputFilePath, fieldValues);

            byte[] pdfAttachment = System.IO.File.ReadAllBytes(outputFilePath);

            await _emailSender.SendSanctionLetter(to, subject, body, pdfAttachment);


            string? url = "";

            if (value != "User")
            {
                url = "/Officer/Index";
            }
            else
            {
                url = "/User/Index";
            }

            return Json(new { status = true, url });
            // return View();
        }


    }
}