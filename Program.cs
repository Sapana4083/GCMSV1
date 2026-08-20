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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// ============================================================
// KESTREL
// ============================================================

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
});


// ============================================================
// DATABASE
// ============================================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseOracle(
        builder.Configuration.GetConnectionString("RcsatOracle"));
});


// ============================================================
// SESSION
// ============================================================

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(240);

    options.Cookie.Name = ".AspNetCore.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

    // CURRENT SERVER IS HTTP
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    // CURRENT SERVER IS HTTPS
    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.Cookie.SameSite = SameSiteMode.Strict;
});


// ============================================================
// REPOSITORIES / SERVICES
// ============================================================

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

builder.Services.AddScoped<ICourtGroupRepository, CourtGroupRepository>();
builder.Services.AddScoped<ICourtGroupService, CourtGroupService>();

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

builder.Services.AddScoped<IDesignationRepository, DesignationRepository>();
builder.Services.AddScoped<IDesignationService, DesignationService>();

builder.Services.AddScoped<IAdvocateRepository, AdvocateRepository>();
builder.Services.AddScoped<IAdvocateService, AdvocateService>();


// ============================================================
// AUTHENTICATION
// ============================================================

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";

        options.Cookie.Name = ".AspNetCore.Cookies";
        options.Cookie.HttpOnly = true;

        // CURRENT SERVER IS HTTP
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        // CURRENT SERVER IS HTTPS
        //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        options.Cookie.SameSite = SameSiteMode.Strict;

        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });


// ============================================================
// MVC + ANTIFORGERY
// ============================================================

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});


// ============================================================
// AUTHORIZATION
// ============================================================

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});


// ============================================================
// ANTIFORGERY
// ============================================================

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";

    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

    // CURRENT SERVER IS HTTP
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    // CURRENT SERVER IS HTTPS
    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.Cookie.SameSite = SameSiteMode.Strict;
});


var app = builder.Build();


// ============================================================
// EXCEPTION HANDLING
// ============================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Account/Error");

    // DO NOT USE HSTS WHILE RUNNING HTTP
    // app.UseHsts();
}


// ============================================================
// IMPORTANT
// ============================================================
// CURRENT SERVER URL:
// http://172.18.177.128/
//
// Therefore DO NOT use:
// app.UseHttpsRedirection();
//
// Enable it only after HTTPS is configured.
// ============================================================

// app.UseHttpsRedirection();


// ============================================================
// SECURITY HEADERS
// ============================================================

app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        var headers = context.Response.Headers;

        // Remove application headers
        headers.Remove("X-Powered-By");
        headers.Remove("X-AspNet-Version");
        headers.Remove("X-AspNetMvc-Version");

        // Security headers
        headers["X-Content-Type-Options"] = "nosniff";

        headers["X-Frame-Options"] = "DENY";

        headers["Referrer-Policy"] =
            "strict-origin-when-cross-origin";

        headers["Permissions-Policy"] =
            "geolocation=(), camera=(), microphone=()";

        return Task.CompletedTask;
    });

    await next();
});


// ============================================================
// STATIC FILES
// ============================================================

var provider = new FileExtensionContentTypeProvider();

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});


// ============================================================
// ROUTING
// ============================================================

app.UseRouting();


// ============================================================
// SESSION
// ============================================================

app.UseSession();


// ============================================================
// AUTHENTICATION
// ============================================================

app.UseAuthentication();


// ============================================================
// AUTHORIZATION
// ============================================================

app.UseAuthorization();


// ============================================================
// DEFAULT ROUTE
// ============================================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();