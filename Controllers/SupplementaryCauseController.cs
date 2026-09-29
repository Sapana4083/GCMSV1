using GCMS.Models;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class SupplementaryCauseController : Controller
    {
        private readonly ISupplementaryCauseService _service;
        private readonly ICaseTypeService _caseTypeService;
        private readonly ICasePurposeService _casePurposeService;
        private readonly ICaseFeedbackService _caseFeedbackService;

        public SupplementaryCauseController(
            ISupplementaryCauseService service,
            ICaseTypeService caseTypeService,
            ICasePurposeService casePurposeService,
            ICaseFeedbackService caseFeedbackService)
        {
            _service = service;
            _caseTypeService = caseTypeService;
            _casePurposeService = casePurposeService;
            _caseFeedbackService = caseFeedbackService;
        }

        [HttpGet]
        public async Task<IActionResult> SupplementaryCauseList()
        {
            ViewBag.CaseTypeList = await _caseTypeService.GetCaseTypeAsync(1, 1000);
            ViewBag.CasePurposeList = await _casePurposeService.GetCasePurposeAsync(1, 1000);

            var data = await _service.GetAllAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetSupplementary(long id)
        {
            var row = await _service.GetByIdAsync(id);
            if (row == null) return Json(null);

            return Json(new
            {
                row.TblSupltyCauseId,
                row.SupplyCauseListId,
                HearingDate = row.HearingDate?.ToString("yyyy-MM-dd"),
                row.CaseTypeId,
                row.CaseNo,
                row.CaseId,
                row.PurposeId,
                row.BenchType
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetCaseNos(long caseTypeId, string? manualCaseNo)
        {
            var rows = await _caseFeedbackService.GetCaseNosAsync(caseTypeId, manualCaseNo, GetCourtCode());
            return Json(rows);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveSupplementary(SupplementaryCause model)
        {
            try
            {
                if (model.HearingDate == null || model.CaseTypeId == null
                    || string.IsNullOrWhiteSpace(model.CaseNo) || model.PurposeId == null)
                {
                    return Json(new { success = false, message = "Hearing Date, Case Type, Case No and Purpose are required." });
                }

                var user = HttpContext.Session.GetString("Username") ?? "SYSTEM";

                if (model.SupplyCauseListId == 0)
                    await _service.AddAsync(model, GetCourtCode(), user);
                else
                    await _service.UpdateAsync(model, GetCourtCode(), user);

                return Json(new { success = true, message = "Saved Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private string GetCourtCode() => HttpContext.Session.GetString("CourtCode") ?? "0";
    }
}