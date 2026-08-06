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
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

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
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



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

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Account/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();