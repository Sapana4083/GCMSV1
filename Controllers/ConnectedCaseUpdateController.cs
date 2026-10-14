using GCMS.Models.ViewModels;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class ConnectedCaseUpdateController : Controller
    {
        private readonly IConnectedCaseService _service;
        private readonly ICaseTypeService _caseTypeService;

        public ConnectedCaseUpdateController(IConnectedCaseService service, ICaseTypeService caseTypeService)
        {
            _service = service;
            _caseTypeService = caseTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.CaseTypeList = await _caseTypeService.GetCaseTypeAsync(1, 1000);
            return View(new ConnectedCaseUpdateViewModel
            {
                CourtCode = HttpContext.Session.GetString("CourtCode")
            });
        }

        //[HttpGet]
        //public async Task<JsonResult> GetByMainCase(string caseNo)
        //{
        //    if (string.IsNullOrWhiteSpace(caseNo)) return Json(new { found = false });

        //    var m = await _service.GetByMainCaseAsync(caseNo.Trim());
        //    if (m == null) return Json(new { found = false });

        //    return Json(new
        //    {
        //        found = true,
        //        m.CaseUpdateId,
        //        m.AppellantName,
        //        m.RespondentName,
        //        InstitutionDate = m.InstitutionDate?.ToString("yyyy-MM-dd"),
        //        HearingDate = m.HearingDate?.ToString("yyyy-MM-dd"),
        //        m.District,
        //        m.PurposeName,
        //        m.CourtCode,
        //        Rows = m.Rows.Select(r => new
        //        {
        //            r.LinkedCaseId,
        //            r.CaseTypeId,
        //            r.CaseNo,
        //            r.MainCase,
        //            HearingDate = r.HearingDate?.ToString("yyyy-MM-dd"),
        //            r.AppellantName,
        //            r.RespondentName,
        //            r.District,
        //            r.Purpose
        //        })
        //    });
        //}

        [HttpGet]
        public async Task<JsonResult> GetChildCase(string caseNo)
        {
            if (string.IsNullOrWhiteSpace(caseNo)) return Json(new { found = false });

            var r = await _service.GetRowByCaseNoAsync(caseNo.Trim());
            if (r == null) return Json(new { found = false });

            return Json(new
            {
                found = true,
                r.LinkedCaseId,
                HearingDate = r.HearingDate?.ToString("yyyy-MM-dd"),
                r.AppellantName,
                r.RespondentName,
                r.District,
                r.Purpose
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetByMainCase(string caseNo, long? caseTypeId)
        {
            if (string.IsNullOrWhiteSpace(caseNo)) return Json(new { found = false });
            caseNo = caseNo.Trim();

            var m = await _service.GetByMainCaseAsync(caseNo);
            var reg = await _service.GetCaseRegDetailsAsync(caseNo, caseTypeId);

            if (m == null && reg == null) return Json(new { found = false });

            m ??= new ConnectedCaseUpdateViewModel();

            // Header: SP ki value, warna Case Registration se
            var appellant = !string.IsNullOrWhiteSpace(m.AppellantName) ? m.AppellantName : reg?.AppellantName;
            var respondent = !string.IsNullOrWhiteSpace(m.RespondentName) ? m.RespondentName : reg?.RespondentName;
            var purpose = !string.IsNullOrWhiteSpace(m.PurposeName) ? m.PurposeName : reg?.Purpose;
            var hearing = m.HearingDate ?? reg?.HearingDate;
            var instDate = m.InstitutionDate?.ToString("yyyy-MM-dd") ?? reg?.CaseTypeName;
            var headerCaseType = m.CaseTypeId ?? reg?.CaseTypeId ?? caseTypeId;

            var rows = new List<object>();
            foreach (var r in m.Rows)
            {
                var childReg = await _service.GetCaseRegDetailsAsync(r.CaseNo ?? "", null);

                rows.Add(new
                {
                    r.LinkedCaseId,
                    CaseTypeId = r.CaseTypeId ?? childReg?.CaseTypeId ?? headerCaseType,
                    r.CaseNo,
                    MainCase = caseNo,
                    HearingDate = (r.HearingDate ?? childReg?.HearingDate ?? hearing)?.ToString("yyyy-MM-dd"),
                    AppellantName = !string.IsNullOrWhiteSpace(r.AppellantName) ? r.AppellantName : (childReg?.AppellantName ?? appellant),
                    RespondentName = !string.IsNullOrWhiteSpace(r.RespondentName) ? r.RespondentName : (childReg?.RespondentName ?? respondent),
                    District = !string.IsNullOrWhiteSpace(r.District) ? r.District : m.District,
                    Purpose = !string.IsNullOrWhiteSpace(r.Purpose) ? r.Purpose : (childReg?.Purpose ?? purpose)
                });
            }

            return Json(new
            {
                found = true,
                m.CaseUpdateId,
                CaseTypeId = headerCaseType,
                AppellantName = appellant,
                RespondentName = respondent,
                InstitutionDate = instDate,
                HearingDate = hearing?.ToString("yyyy-MM-dd"),
                m.District,
                PurposeName = purpose,
                CourtCode = m.CourtCode ?? HttpContext.Session.GetString("CourtCode"),
                Rows = rows
            });
        }

        [HttpGet]
        public async Task<JsonResult> GetChildCase(string caseNo, long? caseTypeId)
        {
            if (string.IsNullOrWhiteSpace(caseNo)) return Json(new { found = false });
            caseNo = caseNo.Trim();

            var r = await _service.GetRowByCaseNoAsync(caseNo);
            var reg = await _service.GetCaseRegDetailsAsync(caseNo, caseTypeId);

            if (r == null && reg == null) return Json(new { found = false });

            return Json(new
            {
                found = true,
                r?.LinkedCaseId,
                CaseTypeId = r?.CaseTypeId ?? reg?.CaseTypeId,
                HearingDate = (r?.HearingDate ?? reg?.HearingDate)?.ToString("yyyy-MM-dd"),
                AppellantName = !string.IsNullOrWhiteSpace(r?.AppellantName) ? r!.AppellantName : reg?.AppellantName,
                RespondentName = !string.IsNullOrWhiteSpace(r?.RespondentName) ? r!.RespondentName : reg?.RespondentName,
                District = r?.District,
                Purpose = !string.IsNullOrWhiteSpace(r?.Purpose) ? r!.Purpose : reg?.Purpose
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] ConnectedCaseUpdateViewModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.LinkCase))
                    return Json(new { success = false, message = "Linked Case (Main Case) is required." });

                if (model.CaseTypeId == null)
                    return Json(new { success = false, message = "Case Type is required." });

                model.Rows = model.Rows.Where(r => !string.IsNullOrWhiteSpace(r.CaseNo)).ToList();
                if (model.Rows.Count == 0)
                    return Json(new { success = false, message = "At least one linked case row is required." });

                model.CourtCode = HttpContext.Session.GetString("CourtCode") ?? model.CourtCode;
                var user = HttpContext.Session.GetString("Username") ?? "SYSTEM";

                var id = await _service.SaveAsync(model, user);
                return Json(new { success = true, caseUpdateId = id, message = "Saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    innerMessage = ex.InnerException?.Message
                });
            }
        }
    }
}