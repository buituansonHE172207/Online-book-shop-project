using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using ShoppingWebApp.CustomHandler;
using ShoppingWebApp.Middlewares;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddGoogle(options =>
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

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Login");

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policyBuilder =>
    {
        policyBuilder.UserRequireCustomClaim(ClaimTypes.Role);
    });
});

builder.Services.AddScoped<IAuthorizationHandler, PoliciesAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(12);
    options.Cookie.Name = ".yourApp.Session";
    options.Cookie.IsEssential = true;
});

builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("DataProtectionKeys"));


builder.Services.AddHttpContextAccessor();

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

