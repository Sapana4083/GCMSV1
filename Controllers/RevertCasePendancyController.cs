using GCMS.Models;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class RevertCasePendancyController : Controller
    {
        private readonly IRevertCasePendancyService _service;

        public RevertCasePendancyController(IRevertCasePendancyService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetByHearingDate(DateTime hearingDate)
        {
            var result = await _service.GetByHearingDateAsync(hearingDate);
            return Json(new { success = result != null, data = result });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Save([FromBody] RevertCasePendancy model)
        {
            try
            {
                if (model.HearingDate == null)
                    return Json(new { success = false, message = "Hearing Date is required." });

                if (model.Cases == null || model.Cases.Count == 0)
                    return Json(new { success = false, message = "At least one case row is required." });

                var user = HttpContext.Session.GetString("Username") ?? "SYSTEM";
                var result = await _service.SaveAsync(model, user);

                return Json(new { success = true, message = "Saved Successfully", data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}