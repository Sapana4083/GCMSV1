using GCMS.Models.ViewModels;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class HearingCauseListController : Controller
    {
        private readonly IHearingCauseListService _service;

        public HearingCauseListController(IHearingCauseListService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? hearingDate, string? benchTypeId, string? causeListType)
        {
            var model = await _service.BuildViewModelAsync(hearingDate, benchTypeId, causeListType);

            if (hearingDate.HasValue && !string.IsNullOrWhiteSpace(benchTypeId))
            {
                model.Rows = await _service.GetRowsAsync(
                    hearingDate.Value, benchTypeId, model.CauseListType, GetCourtCode(), GetUserName());
            }

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetHearingCauseList(DateTime hearingDate, string benchTypeId, string? causeListType)
        {
            var rows = await _service.GetRowsAsync(
                hearingDate, benchTypeId, causeListType ?? "R", GetCourtCode(), GetUserName());

            return Json(rows);
        }

        [HttpPost]
        public async Task<JsonResult> SaveRows([FromBody] SaveHearingCauseListRequest request)
        {
            if (request?.Rows == null || request.Rows.Count == 0)
            {
                return Json(new { success = false, message = "No rows selected for update." });
            }

            if (!request.HearingDate.HasValue || string.IsNullOrWhiteSpace(request.BenchNo))
            {
                return Json(new { success = false, message = "Hearing date and bench no are required." });
            }

            try
            {
                await _service.SaveRowsAsync(request, GetCourtCode(), GetUserName());

                return Json(new
                {
                    success = true,
                    message = "Hearing cause list updated. Appellant and advocate names were not changed."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private string GetCourtCode()
        {
            return HttpContext.Session.GetString("CourtCode") ?? string.Empty;
        }

        private string GetUserName()
        {
            return HttpContext.Session.GetString("Username") ?? User?.Identity?.Name ?? "SYSTEM";
        }
    }
}