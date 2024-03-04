using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServicePlus.Models;
using ServicePlus.Models.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using SendEmails;


namespace ServicePlus.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{

    private readonly ServicePlusContext dbContext;
    private readonly ILogger<HomeController> _logger;

    private readonly IEmailSender _emailSender;


    public HomeController(ServicePlusContext dbContext, ILogger<HomeController> logger, IEmailSender emailSender)
    {
        this.dbContext = dbContext;
        _logger = logger;
        this._emailSender = emailSender;
    }


    public IActionResult Index()
    {
        ClaimsPrincipal claimUser = HttpContext.User;
        if (claimUser!.Identity!.IsAuthenticated)
            return RedirectToAction("Index", "User");

        ViewData["UserType"] = "";
        return View();
    }


    public static string GenerateOtp(int length = 6)
    {
        Random random = new Random();
        const string characters = "0123456789";
        char[] otp = new char[length];

        for (int i = 0; i < length; i++)
        {
            otp[i] = characters[random.Next(characters.Length)];
        }

        return new string(otp);
    }


    public IActionResult Register()
    {
        ViewData["UserType"] = "";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] IFormCollection form)
    {
        var username = form["username"].ToString();
        var email = form["email"].ToString();
        var password = form["password"].ToString();

        string otp = GenerateOtp();

        var usernameParam = new SqlParameter("@Username", username);
        var emailParam = new SqlParameter("@Email", email);
        var passwordParam = new SqlParameter("@Password", password);
        var userTypeParam = new SqlParameter("@UserTypeId", 1);


        var user = dbContext.Users.FromSqlRaw("EXEC InsertUser @Username, @Email, @Password,@UserTypeId", usernameParam, emailParam, passwordParam, userTypeParam).ToList()[0];


        var otpParam = new SqlParameter("@OTP", otp);
        var userIdParam = new SqlParameter("@UserId", user.UserId);

        dbContext.Database.ExecuteSqlRaw("EXEC STOREOTP @OTP,@UserId", otpParam, userIdParam);

        await _emailSender.SendOTP(email!, "OTP VARIFICATION", otp);

        return Json(new { success = true, url = "/Home/OTP", userId = user.UserId });
    }


    public IActionResult OTP(string userId)
    {
        ViewData["UserType"] = "";
        ViewData["userId"] = userId;
        return View();
    }

    [HttpPost]
    public IActionResult OTP([FromForm] IFormCollection form)
    {
        int.TryParse(form["userId"].ToString(), out int userId);
        string otp = form["otp"].ToString();

        _logger.LogInformation($"USERID: {userId}");

        var userIdParam = new SqlParameter("@UserID", userId);

        var otpResult = dbContext.Otpstores.FromSqlRaw("EXEC GETOTP @UserID", userIdParam).ToList()[0];


        if (otp == otpResult.Otp)
        {
            dbContext.Database.ExecuteSqlRaw("EXEC ValidateUser @UserID", userIdParam);
            return RedirectToAction("Login");
        }
        else ViewData["otpError"] = "INVALID OTP";


        return View();
    }

    public IActionResult Login()
    {
        ViewData["UserType"] = "";
        return View();
    }





    [HttpPost]
    public Task<IActionResult> Login([FromBody] User viewModel)
    {
        var usernameParam = new SqlParameter("@InputUsername", viewModel.Username);
        var passwordParam = new SqlParameter("@InputPassword", viewModel.Password);


        var userTypeId = dbContext.Users.FromSqlRaw<User>("EXEC GetUserDetails {0}, {1}", usernameParam, passwordParam).ToList().FirstOrDefault();


        int userTypeIdValue = Convert.ToInt32(userTypeId?.UserTypeId);

        if (userTypeIdValue != -1)
        {
            var userTypeValue = dbContext.UserTypes.FromSqlRaw<UserType>("EXEC GetUserTypeValue @UserTypeId", new SqlParameter("@UserTypeId", userTypeIdValue)).ToList().FirstOrDefault();

            if (userTypeValue?.UserTypeValue != null)
            {
                List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, viewModel.Username),
            };

                AddRoleClaimAndSignIn(userTypeValue.UserTypeValue, claims);

                HttpContext.Session.SetString("UserType", userTypeValue.UserTypeValue);
                HttpContext.Session.SetString("UserId", userTypeId!.UserId.ToString());

                string? url = "";
                if (userTypeValue.UserTypeValue != "User")
                {
                    url = "/Officer/Index";
                }
                else
                {
                    url = "/User/Index";
                }

                return Task.FromResult<IActionResult>(Json(new { status = true, url = url }));
            }
        }

        return Task.FromResult<IActionResult>(Json(new { status = false, message = "Invalid Username or Password." }));
    }

    private async void AddRoleClaimAndSignIn(string role, List<Claim> claims)
    {
        claims.Add(new Claim(ClaimTypes.Role, role));

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        AuthenticationProperties properties = new AuthenticationProperties()
        {
            AllowRefresh = true,
            IsPersistent = true
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
    }


    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult UnauthorizedAccess()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
