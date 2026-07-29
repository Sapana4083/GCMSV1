using GCMS.Models;
using GCMS.Models.Entities;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class CourtMasterController : Controller
    {
        private readonly IDepartmentService _service;
        private readonly ICourtGroupService _courtGroupService;
        public CourtMasterController(IDepartmentService service , ICourtGroupService courtGroupService)
        {
            _service = service;
            _courtGroupService = courtGroupService;
        }

        // List Page
        public async Task<IActionResult> DepartmentList(int pageNo = 1, int rowCnt = 999999)
        {
            var list = await _service.GetAllAsync(pageNo, rowCnt);
            return View(list);
        }

        // GET single record (for Edit modal - AJAX)
        [HttpGet]
        public async Task<IActionResult> GetDepartment(long id)
        {
            var model = await _service.GetByIdAsync(id);

            if (model == null)
                return NotFound();

            return Json(model);
        }

        // Save / Update (AJAX - handles both Add and Edit)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDepartment(DepartmentMaster model)
        {
            if (string.IsNullOrWhiteSpace(model.DepartmentName) ||
                string.IsNullOrWhiteSpace(model.CourtCode))
            {
                return Json(new { success = false, message = "Required fields are missing." });
            }

            var username = HttpContext.Session.GetString("Username") ?? "SYSTEM";

            try
            {
                if (model.DepartmentId > 0)
                {
                    model.ModifiedBy = username;
                    await _service.UpdateAsync(model);
                }
                else
                {
                    model.CreatedBy = username;
                    await _service.SaveAsync(model);
                }

                return Json(new { success = true, message = "Department saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Save failed: " + ex.Message });
            }
        }

        // Delete (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDepartment(long id)
        {
            try
            {
                var username = HttpContext.Session.GetString("Username") ?? "SYSTEM";
                await _service.DeleteAsync(id, username);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Delete failed: " + ex.Message });
            }
        }

        #region Court Group List

        public async Task<IActionResult> CourtGroupList(
            int pageNo = 1,
            int rowCnt = 1000)
        {
            var list = await _courtGroupService.GetAllAsync(pageNo, rowCnt);

            return View(list);
        }

        #endregion

        #region Get Court Group By Id

        [HttpGet]

        public async Task<IActionResult> GetCourtGroup(long id)
        {
            var data = await _courtGroupService.GetByIdAsync(id);

            if (data == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Record not found."
                });
            }

            return Json(new
            {
                success = true,
                courtGroupId = data.CourtGroupId,
                courtGroup = data.CourtGroup,
                courtGroupCode = data.CourtGroupCode,
                inActive = data.InActive
            });
        }

        #endregion

        #region Save Court Group

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCourtGroup(CourtGroupMaster model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid Data"
                });
            }

            if (model.CourtGroupId == 0)
            {
                model.CreatedBy = HttpContext.Session.GetString("Username");

                await _courtGroupService.AddAsync(model);
            }
            else
            {
                model.ModifiedBy = HttpContext.Session.GetString("Username");

                await _courtGroupService.UpdateAsync(model);
            }

            return Json(new
            {
                success = true,
                message = "Court Group Saved Successfully."
            });
        }

        #endregion

        #region Delete Court Group

        [HttpPost]
        public async Task<IActionResult> DeleteCourtGroup(long id)
        {
            try
            {
                var model = await _courtGroupService.GetByIdAsync(id);

                if (model == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Record not found."
                    });
                }

                model.ModifiedBy =
                    HttpContext.Session.GetString("Username") ?? "Admin";

                model.InActive = 1;

                await _courtGroupService.UpdateAsync(model);

                return Json(new
                {
                    success = true,
                    message = "Deleted Successfully."
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

        #endregion
    }
}