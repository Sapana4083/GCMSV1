using GCMS.Models.ViewModels;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class CaseFeedbackController : Controller
    {
        private readonly ICaseTypeService _caseTypeService;
        private readonly ICasePurposeService _casePurposeService;
        private readonly ICaseFeedbackService _caseFeedbackService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CaseFeedbackController(
            ICaseTypeService caseTypeService,
            ICasePurposeService casePurposeService,
            ICaseFeedbackService caseFeedbackService,
            IWebHostEnvironment webHostEnvironment)
        {
            _caseTypeService = caseTypeService;
            _casePurposeService = casePurposeService;
            _caseFeedbackService = caseFeedbackService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await BindDropdowns();
            return View(new CaseFeedbackViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CaseFeedbackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await BindDropdowns();
                return View(model);
            }

            try
            {
                var (fileName, filePath) = await SaveUploadedFile(model.UploadFiles);
                model.SavedFileName = fileName;
                model.SavedFilePath = filePath;

                var userName = HttpContext.Session.GetString("Username") ?? User.Identity?.Name ?? "SYSTEM";

                await _caseFeedbackService.SaveAsync(model, userName);

                TempData["SuccessMessage"] = "Case feedback saved successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await BindDropdowns();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCaseNos(long caseTypeId, string? manualCaseNo)
        {
            var courtCode = HttpContext.Session.GetString("CourtCode") ?? "0";

            var rows = await _caseFeedbackService.GetCaseNosAsync(caseTypeId, manualCaseNo, courtCode);

            return Json(rows);
        }

        private async Task BindDropdowns()
        {
            ViewBag.CaseTypeList = await _caseTypeService.GetCaseTypeAsync(1, 1000);
            ViewBag.CasePurposeList = await _casePurposeService.GetCasePurposeAsync(1, 1000);
        }

        private async Task<(string? FileName, string? FilePath)> SaveUploadedFile(List<IFormFile> files)
        {
            var file = files?.FirstOrDefault(f => f.Length > 0);

            if (file == null)
            {
                return (null, null);
            }

            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "case-feedback");
            Directory.CreateDirectory(uploadPath);

            var storedFileName = $"{Guid.NewGuid():N}_{Path.GetFileName(file.FileName)}";
            var fullPath = Path.Combine(uploadPath, storedFileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return (file.FileName, $"/uploads/case-feedback/{storedFileName}");
        }
    }
}