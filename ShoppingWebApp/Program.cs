using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using ShoppingWebApp.CustomHandler;
using ShoppingWebApp.Middlewares;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
var services = builder.Services;
services.AddControllersWithViews();
services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
    .AddCookie(config =>
    {
        config.LoginPath = "/Login";
        config.LogoutPath = "/Login/LogOut";
        config.AccessDeniedPath = "/Login/UserAccessDenied";
    });

services.ConfigureApplicationCookie(options => options.LoginPath = "/Login");

// Authorization
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie("CookieAuthentication", config =>
    {
        config.Cookie.Name = "UserLoginCookie";
        config.LoginPath = "/Login";
        config.LogoutPath = "/Login/LogOut";
        config.AccessDeniedPath = "/Login/UserAccessDenied";
    });

services.AddAuthorization(config =>
{
    config.AddPolicy("UserPolicy", policyBuilder =>
    {
        policyBuilder.UserRequireCustomClaim(ClaimTypes.Role);

    });
});

services.AddScoped<IAuthorizationHandler, PoliciesAuthorizationHandler>();
services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();

services.AddControllersWithViews();


services.AddDistributedMemoryCache();
services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(12);
    options.Cookie.Name = ".yourApp.Session"; // <--- Add line
    options.Cookie.IsEssential = true;
});


// Session for other
services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<AddRoleIdentityMiddleware>();
app.UseAuthorization();
app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
