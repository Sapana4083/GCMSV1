using GCMS.Models.Entities;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class CourtMasterController : Controller
    {
        private readonly IDepartmentService _service;

        public CourtMasterController(IDepartmentService service)
        {
            _service = service;
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
    }
}