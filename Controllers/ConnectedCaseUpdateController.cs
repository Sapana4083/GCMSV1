using GCMS.Models;
using GCMS.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class ConnectedCaseUpdateController : Controller
    {
        private readonly IRcsatCaseUpdateRepository _repo;
        private readonly ILinkedCaseFamilyRepository _familyRepo;
        private readonly ILinkedCaseRepository _linkedRepo;

        public ConnectedCaseUpdateController(
            IRcsatCaseUpdateRepository repo,
            ILinkedCaseFamilyRepository familyRepo,
            ILinkedCaseRepository linkedRepo)
        {
            _repo = repo;
            _familyRepo = familyRepo;
            _linkedRepo = linkedRepo;
        }

        public IActionResult Index() => View(new RcsatCaseUpdateViewModel());

        [HttpGet]
        public async Task<IActionResult> GetCaseDetails(string linkCase)
        {
            var result = await _repo.GetByLinkCaseAsync(linkCase);
            if (result == null) return Json(new { found = false });

            return Json(new
            {
                found = true,
                caseUpdateId = result.CaseUpdateId,
                courtCode = result.CourtCode,
                caseType = result.CaseType,
                appellantName = result.AppellantName,
                respondentName = result.RespondentName,
                institutionDate = result.InstitutionDate?.ToString("yyyy-MM-dd"),
                hearingDate = result.HearingDate?.ToString("yyyy-MM-dd"),
                district = result.District,
                purposeName = result.PurposeName
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetLinkedCaseFamily(string linkCase, string parentCaseNo, string courtCode)
        {
            if (string.IsNullOrWhiteSpace(parentCaseNo) || string.IsNullOrWhiteSpace(courtCode))
                return BadRequest("Parent case and court code are required.");

            var rows = await _familyRepo.GetCaseFamilyAsync(linkCase, parentCaseNo, courtCode);
            return Json(rows);
        }

        [HttpGet]
        public async Task<IActionResult> CheckCaseExists(string caseNo)
        {
            var exists = await _linkedRepo.CaseExistsAsync(caseNo);
            return Json(new { exists });
        }

        [HttpGet]
        public async Task<IActionResult> GetCaseByNo(string caseNo)
        {
            var row = await _linkedRepo.GetByCaseNoAsync(caseNo);
            return row == null ? Json(new { found = false }) : Json(new { found = true, data = row });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLinkedCase(long caseId)
        {
            var success = await _linkedRepo.DeleteAsync(caseId);
            return Json(new { success });
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] RcsatCaseUpdateViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.LinkCase))
                return BadRequest("Linked Case (Main Case) is required.");

            try
            {
                var caseUpdateId = await _repo.SaveCaseWithLinkedRowsAsync(model);
                return Json(new { success = true, caseUpdateId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
