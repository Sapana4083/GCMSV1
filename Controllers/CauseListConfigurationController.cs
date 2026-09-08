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
        public async Task<IActionResult> Index()
        {
            var model = new CauseListConfigurationViewModel();

            // Cause List Type
            model.CauseListTypes = new List<SelectListItem>
    {
        new SelectListItem
        {
            Value = "R",
            Text = "R"
        },
        new SelectListItem
        {
            Value = "F",
            Text = "F"
        }
    };

            // Get Case Purpose from Oracle
            var casepurpose = await _CasePurposeService
                .GetCasePurposeAsync(1, 1000);

            // Convert to List
            var casePurposeList = casepurpose.ToList();

            // Dropdown data
            ViewBag.CasePurposeList = casePurposeList;

            // Grid rows
            var causeListRows = new List<CauseListCasePurposeViewModel>();

            for (int i = 0; i < 21; i++)
            {
                causeListRows.Add(new CauseListCasePurposeViewModel
                {
                    Id = i + 1,

                    // ⭐ Automatically select Case Purpose
                    CasePurposeId = i < casePurposeList.Count ? Convert.ToInt64(casePurposeList[i].CasePurposeMastId) : null,

                    PurposePriority = 0,
                    DBBenchOne = 50,
                    DBBenchTwo = 50
                });
            }

            ViewBag.CauseListRows = causeListRows;

            return View(model);
        }
        //public async Task<IActionResult> Index()
        //{
        //    var model = new CauseListConfigurationViewModel();

        //    // Cause List Type
        //    model.CauseListTypes = new List<SelectListItem>
        //{
        //    new SelectListItem
        //    {
        //        Value = "R",
        //        Text = "R"
        //    },
        //    new SelectListItem
        //    {
        //        Value = "F",
        //        Text = "F"
        //    }
        //};


        //    // Get Case Purpose from Oracle            

        //    var casepurpose = await _CasePurposeService.GetCasePurposeAsync(1, 1000);

        //    // Dropdown data from database
        //    ViewBag.CasePurposeList = casepurpose;

        //    // Grid rows
        //    var causeListRows = new List<CauseListCasePurposeViewModel>();

        //    for (int i = 0; i < 11; i++)
        //    {
        //        causeListRows.Add(new CauseListCasePurposeViewModel
        //        {
        //            Id = i + 1,
        //            CasePurposeId = null,
        //            PurposePriority = 0,
        //            DBBenchOne = 50,
        //            DBBenchTwo = 50
        //        });
        //    }

        //    ViewBag.CauseListRows = causeListRows;            

        //    return View(model);
        //}
    }
}



