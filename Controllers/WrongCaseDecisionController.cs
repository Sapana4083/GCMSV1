
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class WrongCaseDecisionController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var model = new WrongCaseDecisionViewModel
        {
            CaseTypes = GetCaseTypes()
        };

        return View(model);
    }

    private List<SelectListItem> GetCaseTypes()
    {
        return new List<SelectListItem>
        {
            new SelectListItem
            {
                Value = "1",
                Text = "Appeal"
            },
            new SelectListItem
            {
                Value = "2",
                Text = "Revision"
            },
            new SelectListItem
            {
                Value = "3",
                Text = "Review"
            }
        };
    }

    [HttpGet]
    public IActionResult GetCaseNumbers(string caseType)
    {
        // Sample data; replace with database query.
        var cases = caseType switch
        {
            "1" => new[]
            {
                new { value = "101", text = "101/2026" },
                new { value = "102", text = "102/2026" }
            },
            "2" => new[]
            {
                new { value = "201", text = "201/2026" }
            },
            "3" => new[]
            {
                new { value = "301", text = "301/2026" }
            },
            _ => Array.Empty<dynamic>()
        };

        return Json(cases);
    }

    [HttpGet]
    public IActionResult GetCaseDetails(
        string caseType, string caseNo)
    {
        // Demo response; replace with database data.
        if (caseType == "1" && caseNo == "101")
        {
            return Json(new
            {
                appellantName = "Sample Appellant",
                respondentDepartment = "Revenue Department",
                advocateName = "Sample Advocate",
                decisionDate = "15/10/2026",
                decisionType = "Allowed"
            });
        }

        return Json(null);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Save(
        WrongCaseDecisionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.CaseTypes = GetCaseTypes();
            return View("Index", model);
        }

        // IMPORTANT:
        // Verify selected case exists and is decided.
        // Verify current user's permissions.
        // Update decision status in a transaction.
        // Record audit history before committing.

        // No database update is performed by this demo.

        TempData["Message"] =
            "Demo only: no changes have been saved.";

        return RedirectToAction(nameof(Index));
    }
}
