using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewOnlineVotingApplication.ServiceModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OnlineVotingAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineVotingAppContext") ?? throw new InvalidOperationException("Connection string 'OnlineVotingAppContext' not found.")));
//adding Session to authenticate the application
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".NewOnlineVotingApplication.Session";
    options.IdleTimeout = System.TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Admin/Login";

        options.LoginPath = "/UserRegistrations/Login";
        options.AccessDeniedPath = "/UserRegistrations/Login";
    });

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//using Session in Application
app.UseSession();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserRegistrations}/{action=Login}/{id?}");

app.Run();
