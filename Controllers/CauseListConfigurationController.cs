using GCMS.Models.ViewModels;
using GCMS.Services;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCMS.Controllers
{
    public class CauseListConfigurationController : Controller
    {
        private readonly ICauseListConfigurationService _service;
        private readonly ICasePurposeService _CasePurposeService;
        public CauseListConfigurationController(ICasePurposeService casePurposeService,
            ICauseListConfigurationService service)
        {
            _service = service;
            _CasePurposeService = casePurposeService;
        }


        [HttpGet]
        public async Task<IActionResult> CauseList()
        {
            var data = await _service.GetCauseListAsync();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Index(long? id)
        {
            var casepurpose = await _CasePurposeService.GetCasePurposeAsync(1, 1000);
            var casePurposeList = casepurpose.ToList();
            ViewBag.CasePurposeList = casePurposeList;

            CauseListConfigurationViewModel model;

            if (id.HasValue && id.Value > 0)
            {
                // ✅ EDIT — existing config load karo
                model = await _service.GetCauseListByIdAsync(id.Value)
                        ?? new CauseListConfigurationViewModel();

                ViewBag.CauseListRows = model.CasePurposes;
            }
            else
            {
                // ✅ ADD — default 21 rows
                model = new CauseListConfigurationViewModel();

                var causeListRows = new List<CauseListCasePurposeViewModel>();

                for (int i = 0; i < 21; i++)
                {
                    causeListRows.Add(new CauseListCasePurposeViewModel
                    {
                        Id = i + 1,
                        CasePurposeId = i < casePurposeList.Count ? Convert.ToInt64(casePurposeList[i].CasePurposeMastId) : null,
                        PurposePriority = 0,
                        DBBenchOne = 50,
                        DBBenchTwo = 50
                    });
                }

                ViewBag.CauseListRows = causeListRows;
            }

            model.CauseListTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "R", Text = "R" },
                new SelectListItem { Value = "F", Text = "F" }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CauseListConfigurationViewModel model)
        {
            try
            {
                var username = HttpContext.Session.GetString("Username") ?? "SYSTEM";

                var id = await _service.SaveCauseListAsync(model, username);

                TempData["SuccessMessage"] = "Cause List Configuration saved successfully.";

                return RedirectToAction("CauseList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", new { id = model.CLPNo });
            }
        }

    }
}