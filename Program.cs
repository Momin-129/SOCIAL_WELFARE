using Microsoft.EntityFrameworkCore;
using ServicePlus.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using SendEmails;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Home/Login";
    options.AccessDeniedPath = "/Home/UnauthorizedAccess";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CitizenPolicy", policy => policy.RequireRole("User"));
    options.AddPolicy("TSWOPolicy", policy => policy.RequireRole("TSWO"));
    options.AddPolicy("DSWOPolicy", policy => policy.RequireRole("DSWO"));
    options.AddPolicy("DirectorFinPolicy", policy => policy.RequireRole("DirectorFin"));
    options.AddPolicy("DDCPolicy", policy => policy.RequireRole("DDC"));
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});



builder.Services.AddScoped<HelperFunction>();
builder.Services.AddScoped<PdfFillService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddDbContext<ServicePlusContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();


app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
