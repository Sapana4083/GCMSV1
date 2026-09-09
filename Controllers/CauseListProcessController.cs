using GCMS.Models.ViewModels;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class CauseListProcessController : Controller
    {
        private readonly ICauseListProcessService _service;

        public CauseListProcessController(ICauseListProcessService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var courtCode = HttpContext.Session.GetString("CourtCode") ?? "0";

            var model = new CauseListGenerateViewModel
            {
                CourtCode = courtCode,
                DocDate = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Generate(CauseListGenerateViewModel model)
        {
            try
            {
                model.CourtCode = HttpContext.Session.GetString("CourtCode") ?? model.CourtCode;

                await _service.GenerateCauseListAsync(model);

                return Json(new
                {
                    success = true,
                    message = "Cause List Generated Successfully."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetGeneratedList(DateTime hearingDate)
        {
            var courtCode = HttpContext.Session.GetString("CourtCode") ?? "0";

            var data = await _service.GetGeneratedListAsync(hearingDate, courtCode);

            return Json(data);
        }
    }
}