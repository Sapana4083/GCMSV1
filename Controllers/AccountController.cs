using GCMS.Models;
using GCMS.Repository.Interfaces;
using GCMS.Services.Interfaces;
using GCMS.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace GCMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _repository;

        public AccountController(
            IAuthService authService, IUserRepository repository)
        {
            _authService = authService;
            _repository = repository;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool isValid = await _authService.ValidateUserAsync(
                model.UserName,
                model.Password);

            if (!isValid)
            {
                TempData["Error"] = "Invalid Username or Password";
                return View(model);
            }

            // Get Department & Court Details
            var deptInfo = await _repository.GetDepartmentAndCourtAsync(
                model.UserName);

            // Create Claims
            var claims = new List<Claim>
            {
              new Claim(ClaimTypes.Name, model.UserName)
            };

            if (deptInfo != null)
            {
                claims.Add(new Claim("UserSSO", deptInfo.UserSSO ?? ""));
                claims.Add(new Claim("AxUserID", deptInfo.AxUserID ?? ""));
                claims.Add(new Claim("DepartmentName", deptInfo.DepartmentName ?? ""));
                claims.Add(new Claim("CourtName", deptInfo.CourtName ?? ""));
                claims.Add(new Claim("CourtCode", deptInfo.CourtCode ?? ""));
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            // Save Session
            HttpContext.Session.SetString(
                "Username",
                model.UserName);

            if (deptInfo != null)
            {
                HttpContext.Session.SetString("UserSSO", deptInfo.UserSSO ?? "");
                HttpContext.Session.SetString("AxUserID", deptInfo.AxUserID ?? "");
                HttpContext.Session.SetString("DepartmentName", deptInfo.DepartmentName ?? "");
                HttpContext.Session.SetString("CourtName", deptInfo.CourtName ?? "");
                HttpContext.Session.SetString("CourtCode", deptInfo.CourtCode ?? "");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
