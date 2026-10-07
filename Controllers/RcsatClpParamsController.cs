using GCMS.Models;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class RcsatClpParamsController : Controller
    {
        private readonly IRcsatClpParamsService _service;

        public RcsatClpParamsController(IRcsatClpParamsService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new List<RcsatClpParams>());
        }

        [HttpGet]
        public async Task<JsonResult> Search(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = await _service.SearchAsync(fromDate, toDate);
                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}