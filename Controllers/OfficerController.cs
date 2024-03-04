using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServicePlus.Models.Entities;

namespace ServicePlus.Controllers
{
    [Authorize(Roles = "TSWO,DSWO,DirectorFin,DDC")]
    public class OfficerController : Controller
    {
        private readonly ServicePlusContext dbContext;
        private readonly ILogger<OfficerController> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly PdfFillService _pdfFileService;

        public OfficerController(ServicePlusContext dbContext, ILogger<OfficerController> logger, PdfFillService pdfFillService, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            _logger = logger;
            _pdfFileService = pdfFillService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ViewData["UserType"] = "Officer";

            string? userType = HttpContext.Session.GetString("UserType");
            string? phaseKey = "";
            string? phaseValue = "";
            string? fieldKey = "";
            int fieldValue = 0;

            if (userType == "TSWO")
            {
                phaseKey = "Tehsil Social Welfare Officer";
                phaseValue = "Pending";
                fieldKey = "Teshil";
                fieldValue = 150;
            }
            else if (userType == "DSWO")
            {
                phaseKey = "District Social Welfare Officer";
                phaseValue = "Pending";
                fieldKey = "District";
                fieldValue = 15;
            }
            else if (userType == "DirectorFin")
            {
                phaseKey = "Director Of Finance";
                phaseValue = "Pending";
                fieldKey = "District";
                fieldValue = 15;
            }
            else if (userType == "DDC")
            {
                phaseKey = "DDC";
                phaseValue = "Pending";
                fieldKey = "District";
                fieldValue = 15;
            }
            // Add more conditions for other user types as needed

            var PhaseKey = new SqlParameter("@PhaseKey", phaseKey);
            var PhaseValue = new SqlParameter("@PhaseValue", phaseValue);
            var FieldKey = new SqlParameter("@FieldKey", fieldKey);
            var FieldValue = new SqlParameter("@FieldValue", fieldValue);


            var citizenList = dbContext.CitizenDetails
                .FromSqlRaw("EXEC GetCitizenDetailsByPhase @PhaseKey, @PhaseValue, @FieldKey, @FieldValue", PhaseKey, PhaseValue, FieldKey, FieldValue)
                .ToList();

            return View(citizenList);
        }




    }
}