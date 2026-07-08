using GCMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCMS.Controllers
{
    public class CaseRegistrationController : Controller
    {
        public IActionResult Step1()
        {
            var vm = new CaseRegistrationWizardViewModel();

            vm.CaseTypes = new List<SelectListItem>()
            {
                new SelectListItem { Value="1", Text="Appeal"},
                new SelectListItem { Value="2", Text="Revision"}
            };

            vm.CaseSubjects = new List<SelectListItem>()
            {
                new SelectListItem { Value="1", Text="Land"}
            };

            vm.CasePurposes = new List<SelectListItem>()
            {
                new SelectListItem { Value="1", Text="Registration"}
            };

            vm.BenchTypes = new List<SelectListItem>()
            {
                new SelectListItem { Value="1", Text="Single Bench"}
            };

            return View(vm);
        }
        [HttpPost]
        public IActionResult Step1(CaseRegistrationWizardViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction("Step2");
        }

        public IActionResult Step2()
        {
            return View();
        }

        public IActionResult Step3()
        {
            return View();
        }

        public IActionResult Step4()
        {
            return View();
        }
    }
}
