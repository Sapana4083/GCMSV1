using GCMS.Data;
using GCMS.Models;
using GCMS.Repositories;
using GCMS.Repository;
using GCMS.Repository.Implementations;
using GCMS.Repository.Interfaces;
using GCMS.Services;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
});

builder.Services.AddDbContext<ApplicationDbContext>(
options =>
{
    options.UseOracle(
        builder.Configuration
        .GetConnectionString("RcsatOracle"));
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});



builder.Services.AddScoped<OracleConnectionFactory>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<IDivisionRepository, DivisionRepository>();
builder.Services.AddScoped<IDivisionService, DivisionService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IRoleMenuMappingRepository, RoleMenuMappingRepository>();
builder.Services.AddScoped<IRoleMenuMappingService, RoleMenuMappingService>();
builder.Services.AddScoped<ITehsilRepository, TehsilRepository>();
builder.Services.AddScoped<ITehsilService, TehsilService>();
builder.Services.AddScoped<ISdoRepository, SdoRepository>();
builder.Services.AddScoped<ISdoService, SdoService>();
builder.Services.AddScoped<ICourtDashboardRepository, CourtDashboardRepository>();

builder.Services.AddScoped<ICourtTypeRepository, CourtTypeRepository>();
builder.Services.AddScoped<ICourtTypeService, CourtTypeService>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddScoped<ICourtGroupRepository,CourtGroupRepository>();
builder.Services.AddScoped<ICourtGroupService,CourtGroupService>();

builder.Services.AddScoped<ICasePurposeRepository, CasePurposeRepository>();
builder.Services.AddScoped<ICasePurposeService, CasePurposeService>();

builder.Services.AddScoped<ICasePurposeGroupRepository, CasePurposeGroupRepository>();
builder.Services.AddScoped<ICasePurposeGroupService, CasePurposeGroupService>();

builder.Services.AddScoped<ICaseRepository, CaseRepository>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<ILinkedCaseFamilyRepository, LinkedCaseFamilyRepository>();

builder.Services.AddScoped<IRcsatCaseUpdateRepository, RcsatCaseUpdateRepository>();
builder.Services.AddScoped<ILinkedCaseRepository, LinkedCaseRepository>();

builder.Services.AddScoped<ICaseTypeRepository, CaseTypeRepository>();
builder.Services.AddScoped<ICaseTypeService, CaseTypeService>();

builder.Services.AddScoped<IBenchTypeRepository, BenchTypeRepository>();
builder.Services.AddScoped<IBenchTypeService, BenchTypeService>();

builder.Services.AddScoped<ICaseSubjectRepository, CaseSubjectRepository>();
builder.Services.AddScoped<ICaseSubjectService, CaseSubjectService>();

//builder.Services.AddAuthentication(
//    CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Account/Login";
//    });


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied"; // add if you have one
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // match your session idle timeout
        options.SlidingExpiration = true;
    });

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});


builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});



builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";
    options.Cookie.SameSite = SameSiteMode.Strict; // or Lax
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
});

var app = builder.Build();
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        var headers = context.Response.Headers;
        headers.Remove("Server");
        headers.Remove("X-Powered-By");
        headers.Remove("X-AspNet-Version");
        headers.Remove("X-AspNetMvc-Version");

        headers["X-Content-Type-Options"] = "nosniff";
        headers["X-Frame-Options"] = "DENY";
        headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        headers["Permissions-Policy"] = "geolocation=(), camera=(), microphone=()";

        var connectSrc = app.Environment.IsDevelopment()
            ? "connect-src 'self' ws: wss: http://localhost:* https://localhost:*;"
            : "connect-src 'self';";

        headers["Content-Security-Policy"] =
            "default-src 'self'; " +
            //"script-src 'self' 'unsafe-eval' blob:; " +
            "script-src 'self' 'unsafe-inline' 'unsafe-eval' blob:; " +
            "style-src 'self' https://cdnjs.cloudflare.com 'unsafe-inline'; " +
            "img-src 'self' data: blob:; " +
            "font-src 'self' https://cdnjs.cloudflare.com data:; " +
            connectSrc + " " +
            "object-src 'none'; " +
            "worker-src 'self' blob:;";

        return Task.CompletedTask;
    });
    await next();
});


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Account/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".css"] = "text/css";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

//app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();