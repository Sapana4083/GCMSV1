using GCMS.Models;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class VcLinkController : Controller
    {
        private readonly IVcLinkService _service;

        public VcLinkController(IVcLinkService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> VcLinkList()
        {
            var data = await _service.GetAllAsync(GetCourtCode());
            return View(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetVcLink(long id)
        {
            var row = await _service.GetByIdAsync(id);
            if (row == null) return Json(null);

            return Json(new
            {
                row.TrnRcsatVclinkId,
                HearingDate = row.HearingDate?.ToString("yyyy-MM-dd"),
                row.Type,
                row.Bench,
                row.VcLinkUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveVcLink(VcLink model)
        {
            try
            {
                if (model.HearingDate == null || string.IsNullOrWhiteSpace(model.Type)
                    || string.IsNullOrWhiteSpace(model.Bench) || string.IsNullOrWhiteSpace(model.VcLinkUrl))
                {
                    return Json(new { success = false, message = "Hearing Date, Cause List Type, Bench Number and VC Link are required." });
                }

                var user = HttpContext.Session.GetString("Username") ?? "SYSTEM";
                var courtCode = GetCourtCode();

                if (model.TrnRcsatVclinkId == 0)
                    await _service.AddAsync(model, courtCode, user);
                else
                    await _service.UpdateAsync(model, courtCode, user);

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