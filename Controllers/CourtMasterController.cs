using GCMS.Models;
using GCMS.Models.Entities;
using GCMS.Services;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCMS.Controllers
{
    public class CourtMasterController : Controller
    {
        private readonly IDepartmentService _service;
        private readonly ICourtGroupService _courtGroupService;
        private readonly ICourtTypeService _courtTypeService;
        private readonly ICasePurposeGroupService _casePurposeGroupService;
        private readonly ICasePurposeService _casePurposeService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly IBenchTypeService _benchTypeService;
        private readonly ICaseSubjectService _caseSubjectService;
        private readonly IDesignationService _designationService;
        private readonly IAdvocateService _advocateService; 

        private readonly IRcsatDepartmentService _rcsatDepartmentService;

        public CourtMasterController(
            IDepartmentService service,
            ICourtGroupService courtGroupService,
            ICourtTypeService courtTypeService,
            ICasePurposeGroupService casePurposeGroupService,
            ICasePurposeService casePurposeService,
            ICaseTypeService caseTypeService,
            IBenchTypeService benchTypeService,
            ICaseSubjectService caseSubjectService,
            IDesignationService designationService,
            IAdvocateService advocateService,
            IRcsatDepartmentService rcsatDepartmentService)
        {
            _service = service;
            _courtGroupService = courtGroupService;
            _courtTypeService = courtTypeService;
            _casePurposeGroupService = casePurposeGroupService;
            _casePurposeService = casePurposeService;
            _caseTypeService = caseTypeService;
            _benchTypeService = benchTypeService;
            _caseSubjectService = caseSubjectService;
            _designationService = designationService;
            _advocateService = advocateService;
            _rcsatDepartmentService = rcsatDepartmentService;
        }

        #region Department
        // List Page
        public async Task<IActionResult> DepartmentList(int pageNo = 1, int rowCnt = 999999)
        {
            var list = await _service.GetAllAsync(pageNo, rowCnt);
            return View(list);
        }
        public async Task<IActionResult> OrderDepartmentList(int pageNo = 1, int rowCnt = 999999)
        {
            var list = await _rcsatDepartmentService.GetDepartmentNameListAsync();
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
        #endregion

        #region Court Group List

        public async Task<IActionResult> CourtGroupList(
            int pageNo = 1,
            int rowCnt = 1000)
        {
            var list = await _courtGroupService.GetAllAsync(pageNo, rowCnt);

            return View(list);
        }

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

        #region Court Group

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

        #region Court Type
        public async Task<IActionResult> CourtTypeList(int pageNo = 1, int rowCnt = 999999)
        {
            var list = await _courtTypeService.GetAllAsync(pageNo, rowCnt);

            var departments = await _service.GetAllAsync(1, 999999);
            var courtGroup = await _courtGroupService.GetAllAsync(1, 999999);
            var categoryList = await _courtTypeService.GetCourtCategoryAsync();

            ViewBag.DepartmentList = new SelectList(
       departments,
       "DepartmentId",
       "DepartmentName"
   );
            ViewBag.GroupList = new SelectList(
     courtGroup,
     "CourtGroupCode",
     "CourtGroup"
 );

            ViewBag.CourtCategoryList = new SelectList(
       categoryList,
       "Id",
       "Name"
   );



            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourtType(long id)
        {
            var model = await _courtTypeService.GetByIdAsync(id);

            if (model == null)
                return NotFound();

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCourtType(CourtTypeMaster model)
        {
            if (model.CourtTypeMastId == 0)
            {
                model.CreatedBy = HttpContext.Session.GetString("Username");

                await _courtTypeService.AddAsync(model);
            }
            else
            {
                model.ModifiedBy = HttpContext.Session.GetString("Username");

                await _courtTypeService.UpdateAsync(model);
            }

            return Json(new
            {
                success = true,
                message = "Saved Successfully."
            });
        }
        #endregion

        #region Case Purpose Group List
        public async Task<IActionResult> CasePurposeGroupList(
    int pageNo = 1,
    int rowCnt = 999999)
        {
            var list = await _casePurposeGroupService.GetAllAsync(pageNo, rowCnt);

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetCasePurposeGroup(long id)
        {
            var model = await _casePurposeGroupService.GetByIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCasePurposeGroup(CasePurposeGroupMaster model)
        {
            try
            {
                if (model.CasePurposeGroupMastId == 0)
                {
                    model.CreatedBy = HttpContext.Session.GetString("Username");

                    await _casePurposeGroupService.AddAsync(model);
                }
                else
                {
                    model.ModifiedBy = HttpContext.Session.GetString("Username");

                    await _casePurposeGroupService.UpdateAsync(model);
                }

                return Json(new
                {
                    success = true,
                    message = "Saved Successfully"
                });
            }
            catch (Exception ex)
            {
                // TODO: proper logging (ILogger) yahan add karo production ke liye
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> DeleteCasePurposeGroup(long id)
        //{
        //    var user = HttpContext.Session.GetString("Username");

        //    await _casePurposeGroupService.DeleteAsync(id, user);

        //    return Json(new
        //    {
        //        success = true
        //    });
        //}

        #endregion

        #region Case Purpose 
        // ── LIST PAGE ──────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> CasePurposeList(int pageNo = 1, int rowCnt = 999999)
        {
            var list = await _casePurposeService.GetAllAsync(pageNo, rowCnt);

            var groupList = await _casePurposeGroupService.GetAllAsync(1, 999999);

            ViewBag.GroupList = new SelectList(
                groupList,
                "CasePurposeGroupMastId",
                "CasePurposeGroup"
            );

            return View(list);
        }

        // ── GET BY ID (for edit modal) ────────────────
        [HttpGet]
        public async Task<IActionResult> GetCasePurpose(long id)
        {
            var data = await _casePurposeService.GetByIdAsync(id);
            if (data == null)
                return Json(null);

            return Json(data);
        }

        // ── SAVE (Insert / Update) ────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCasePurpose(CasePurposeMaster model)
        {
            try
            {
                // TODO: apna session-based username yaha se lo (jaise baaki modules me hai)
                var userName = HttpContext.Session.GetString("Username") ?? "SYSTEM";

                if (model.CasePurposeMastId == 0)
                {
                    model.CreatedBy = userName;
                    await _casePurposeService.AddAsync(model);
                }
                else
                {
                    model.UserName = userName;
                    await _casePurposeService.UpdateAsync(model);
                }

                return Json(new { success = true, message = "Record saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Case Type
        // Case Type Master
        public async Task<IActionResult> CaseTypeList(int pageNo = 1, int rowCnt = 999999999)
        {
            var data = await _caseTypeService.GetAllAsync(pageNo, rowCnt);

            ViewBag.PageNo = pageNo;
            ViewBag.RowCnt = rowCnt;

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveCaseType(CaseTypeMaster model)
        {
            try
            {
                if (model.CaseTypeMastId == 0)
                {
                    var username = HttpContext.Session.GetString("Username") ?? "SYSTEM";
                    model.CreatedBy = username;

                    await _caseTypeService.AddAsync(model);
                }
                else
                {
                    var username = HttpContext.Session.GetString("Username") ?? "SYSTEM";
                    model.CreatedBy = username;

                    await _caseTypeService.UpdateAsync(model);
                }

                return Json(new
                {
                    success = true,
                    message = "Saved Successfully"
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
        public async Task<JsonResult> GetCaseType(long id)
        {
            var caseType = await _caseTypeService.GetByIdAsync(id);
            return Json(caseType);
        }
        #endregion

        #region Bench Type
        // Bench Type Master
        public async Task<IActionResult> BenchTypeList(int pageNo = 1, int rowCnt = 999999999)
        {
            var data = await _benchTypeService.GetAllAsync(pageNo, rowCnt);

            var courtList = await _courtTypeService.GetAllAsync(1, 999999); // apna actual Court service

            ViewBag.CourtList = new SelectList(
                courtList,
                "CourtTypeMastId",  // CourtMaster model ki actual ID property
                "CourtTypeName"     // CourtMaster model ki actual Name property
            );

            ViewBag.PageNo = pageNo;
            ViewBag.RowCnt = rowCnt;

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveBenchType(BenchTypeMaster model)
        {
            try
            {
                var username = HttpContext.Session.GetString("Username") ?? "SYSTEM";

                if (model.BenchTypeMastId == 0)
                {
                    model.CreatedBy = username;
                    await _benchTypeService.AddAsync(model);
                }
                else
                {
                    model.UserName = username;
                    await _benchTypeService.UpdateAsync(model);
                }

                return Json(new
                {
                    success = true,
                    message = "Saved Successfully"
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
        public async Task<JsonResult> GetBenchType(long id)
        {
            var benchType = await _benchTypeService.GetByIdAsync(id);
            return Json(benchType);
        }
        #endregion

        #region Case Subject
        // Case Subject Master
        public async Task<IActionResult> CaseSubjectList(int pageNo = 1, int rowCnt = 999999999)
        {
            var data = await _caseSubjectService.GetAllAsync(pageNo, rowCnt);

            ViewBag.PageNo = pageNo;
            ViewBag.RowCnt = rowCnt;

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveCaseSubject(CaseSubjectMaster model)
        {
            try
            {
                var username = HttpContext.Session.GetString("Username") ?? "SYSTEM";

                if (model.CaseSubjectId == 0)
                {
                    model.CreatedBy = username;
                    await _caseSubjectService.AddAsync(model);
                }
                else
                {
                    model.UserName = username;
                    await _caseSubjectService.UpdateAsync(model);
                }

                return Json(new
                {
                    success = true,
                    message = "Saved Successfully"
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
        public async Task<JsonResult> GetCaseSubject(long id)
        {
            var caseSubject = await _caseSubjectService.GetByIdAsync(id);
            return Json(caseSubject);
        }
        #endregion

        #region Designation
        // Designation Master
        public async Task<IActionResult> DesignationList(int pageNo = 1, int rowCnt = 999999999)
        {
            var data = await _designationService.GetAllAsync(pageNo, rowCnt);

            ViewBag.PageNo = pageNo;
            ViewBag.RowCnt = rowCnt;

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveDesignation(DesignationMaster model)
        {
            try
            {
                var username = HttpContext.Session.GetString("Username") ?? "SYSTEM";

                if (model.CmRcsatDesignTmpId == 0)
                {
                    model.CreatedBy = username;
                    await _designationService.AddAsync(model);
                }
                else
                {
                    model.CreatedBy = username; // P_MODIFIEDBY ke liye reuse
                    await _designationService.UpdateAsync(model);
                }

                return Json(new
                {
                    success = true,
                    message = "Saved Successfully"
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
        public async Task<JsonResult> GetDesignation(long id)
        {
            var designation = await _designationService.GetByIdAsync(id);
            return Json(designation);
        }
        #endregion

        #region Advocate
        // Advocate Master
        public async Task<IActionResult> AdvocateList(int pageNo = 1, int rowCnt = 999999999)
        {
            var data = await _advocateService.GetAllAsync(pageNo, rowCnt);

            var departments = await _service.GetAllAsync(1, 999999);
            ViewBag.DepartmentList = new SelectList(
                departments,
                "DepartmentId",
                "DepartmentName"
            );

            var courtList = await _courtTypeService.GetAllAsync(1, 999999);
            ViewBag.CourtList = new SelectList(
                courtList,
                "CourtTypeMastId",
                "CourtTypeName"
            );

            ViewBag.PageNo = pageNo;
            ViewBag.RowCnt = rowCnt;

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveAdvocate(AdvocateMaster model)
        {
            try
            {
                var username = HttpContext.Session.GetString("Username") ?? "SYSTEM";
                model.CreatedBy = username;

                if (model.MastRcsatAdvocateId == 0)
                {
                    await _advocateService.AddAsync(model);
                }
                else
                {
                    await _advocateService.UpdateAsync(model);
                }

                return Json(new
                {
                    success = true,
                    message = "Saved Successfully"
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
        public async Task<JsonResult> GetAdvocate(long id)
        {
            var advocate = await _advocateService.GetByIdAsync(id);
            return Json(advocate);
        }
        #endregion

#region RCSAT Department Master
        public async Task<IActionResult> RcsatDepartmentList(int pageNo = 1, int rowCnt = 999999999)
        {
            var data = await _rcsatDepartmentService.GetAllAsync(pageNo, rowCnt);

            ViewBag.PageNo = pageNo;
            ViewBag.RowCnt = rowCnt;

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveRcsatDepartment(RcsatDepartmentMaster model)
        {
            try
            {
                var username = HttpContext.Session.GetString("Username") ?? "SYSTEM";

                if (model.CmRcsatDeptId == 0)
                {
                    model.CreatedBy = username;
                    await _rcsatDepartmentService.AddAsync(model);
                }
                else
                {
                    model.ModifiedBy = username;
                    await _rcsatDepartmentService.UpdateAsync(model);
                }

                return Json(new
                {
                    success = true,
                    message = "Saved Successfully"
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
        public async Task<JsonResult> GetRcsatDepartment(long id)
        {
            var department = await _rcsatDepartmentService.GetByIdAsync(id);
            return Json(department);
        }
        #endregion
    }
}