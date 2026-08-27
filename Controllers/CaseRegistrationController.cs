using GCMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GCMS.Models.Entities;
using GCMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using GCMS.Data;
using GCMS.Helpers;
using GCMS.Models.Common;
using GCMS.Services;
using System.Globalization;
using System.Linq;


namespace GCMS.Controllers
{
    public class CaseRegistrationController : Controller
    {
        private const string SaveErrorMessage = "Unable to save case registration details. Please try again or contact support.";
        private readonly ICaseService _caseService;
        private readonly IDepartmentService _service;
        private readonly IRcsatDepartmentService _RcsatDeptservice;
        private readonly ICaseTypeService _CaseTypeservice;
        private readonly ICaseSubjectService _CaseSubjectService;
        private readonly ICasePurposeService _CasePurposeService;
        private readonly IBenchTypeService _BenchTypeService;
        private readonly IDistrictService _DistrictService;
        private readonly IDesignationService _DesignationService;
        private readonly IAdvocateService _AdvocateService;
        private readonly ILogger<CaseRegistrationController> _logger;
        

        public CaseRegistrationController(IDepartmentService service,IRcsatDepartmentService rcsatDeptservice, ICaseService caseService, ICaseTypeService caseTypeService, ICaseSubjectService caseSubjectService,ICasePurposeService casePurposeService,IBenchTypeService benchTypeService,IDistrictService districtService,IDesignationService designationService,IAdvocateService advocateService,ILogger<CaseRegistrationController> logger)
        {
            _service = service;
            _RcsatDeptservice = rcsatDeptservice;
            _caseService = caseService;
            _logger = logger;
            _CaseTypeservice = caseTypeService;
            _CaseSubjectService = caseSubjectService;
            _CasePurposeService = casePurposeService;
            _BenchTypeService = benchTypeService;
            _DistrictService = districtService;
            _DesignationService = designationService;
            _AdvocateService = advocateService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new CaseRegistrationWizardViewModel();

            await  BindDropdowns(vm);

            return View(vm);
        }
        //=========================
        // STEP 1 
        //=========================
        //[HttpPost]
        //public async Task<JsonResult> SaveStep1(CaseRegistrationWizardViewModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return Json(new AjaxResponse
        //            {
        //                Success = false,
        //                Message = string.Join("<br/>",
        //                ModelState.Values
        //                .SelectMany(v => v.Errors)
        //                .Select(e => e.ErrorMessage))
        //            });
        //        }

        //        var entity = new CaseRegistration
        //        {
        //            InstitutionDate = model.InstitutionDate,
        //            CaseNo = model.CaseNumber,
        //            //ManualCaseNo = model.ManualCaseNumber,
        //            OrderNo = model.OrderNumber,
        //            DateOfOrder = model.DateofImpugnedOrder,
        //            OrderIssuedById = model.OrderIssuedById,
        //            CourtCode = model.CourtCode,
        //            CaseTypeId = model.CaseTypeId,
        //            CaseSubjectId = model.CaseSubjectId,
        //            CasePurposeId = model.CasePurposeId,
        //            HearingDate = model.HearingDate,
        //            BenchTypeId = model.BenchTypeId,
        //            LinkedCase = model.LinkedCaseNumber,
        //            OldCaseNo = model.OldCaseNumber,
        //            CreatedBy = User.Identity?.Name ?? "ADMIN"
        //        };
        //        long caseId = SessionHelper.GetCaseId(HttpContext);

        //        if (caseId == 0)
        //        {
        //            caseId = await _caseService.SaveCaseAsync(entity);
        //            SessionHelper.SetCaseId(HttpContext, caseId);
        //        }
        //        else
        //        {
        //            entity.CaseId = caseId;
        //            await _caseService.UpdateCaseAsync(entity);
        //        }

        //        //long caseId = await _caseService.SaveCaseAsync(entity);

        //        SessionHelper.SetCaseId(HttpContext, caseId);

        //        return Json(new AjaxResponse
        //        {
        //            Success = true,
        //            CaseId = caseId,
        //            Message = "Step1 Saved Successfully"
        //        });

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error saving case registration step 1.");
        //        return Json(new AjaxResponse
        //        {
        //            Success = false,
        //            Message = SaveErrorMessage
        //        });
        //    }
        //}

        //[HttpPost]
        //public async Task<JsonResult> SaveStep2(CaseRegistrationWizardViewModel model)
        //{
        //    try
        //    {
        //        long caseId = SessionHelper.GetCaseId(HttpContext);

        //        if (caseId == 0)
        //        {
        //            return Json(new AjaxResponse
        //            {
        //                Success = false,
        //                Message = "Session Expired"
        //            });
        //        }

        //        var entity = new CaseAppellant
        //        {
        //            CaseId = caseId,
        //            AppellantName = model.AppellantName,
        //            Designation = model.DesignationId?.ToString(),
        //            District = model.DistrictId?.ToString(),
        //            MobileNo = model.MobileNumber,
        //            EmployeeId = model.EmployeeId,
        //            AdvocateId = model.AdvocateId,
        //            AdvocateEmail = model.AdvocateEmail,
        //            AdvocateMobile = model.AdvocateMobile?.ToString()
        //        };

        //        await _caseService.SaveAppellantAsync(entity);

        //        return Json(new AjaxResponse
        //        {
        //            Success = true,
        //            Message = "Step2 Saved Successfully"
        //        });

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error saving case registration step 1.");
        //        return Json(new AjaxResponse
        //        {
        //            Success = false,
        //            Message = SaveErrorMessage
        //        });
        //    }
        //}

        //[HttpPost]
        //public async Task<JsonResult> SaveStep3(CaseRegistrationWizardViewModel model)
        //{
        //    try
        //    {
        //        long caseId = SessionHelper.GetCaseId(HttpContext);

        //        if (caseId == 0)
        //        {
        //            return Json(new AjaxResponse
        //            {
        //                Success = false,
        //                Message = "Session Expired"
        //            });
        //        }

        //        var entity = new CaseRespondent
        //        {
        //            CaseId = caseId,
        //            DepartmentId = model.DepartmentId,
        //            AdvocateId = model.RespondentAdvocateId,
        //            AdvocateEmail = model.RespondentAdvocateEmail,
        //            AdvocateMobile = model.RespondentAdvocateMobile,
        //            CreatedBy = "ADMIN"
        //        };

        //        await _caseService.SaveRespondentAsync(entity);

        //        return Json(new AjaxResponse
        //        {
        //            Success = true,
        //            Message = "Step3 Saved Successfully"
        //        });

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error saving case registration step 1.");
        //        return Json(new AjaxResponse
        //        {
        //            Success = false,
        //            Message = SaveErrorMessage
        //        });
        //    }
        //}

        //[HttpPost]
        //public async Task<JsonResult> SaveStep4(CaseRegistrationWizardViewModel model)
        //{
        //    try
        //    {
        //        long caseId = SessionHelper.GetCaseId(HttpContext);

        //        var entity = new CasePrivateParty
        //        {
        //            CaseId = caseId,
        //            PartyName = model.PrivatePartyName,
        //            Designation = model.PrivateDesignation,
        //            Advocate = model.AdvocateId,
        //            CreatedBy = User.Identity?.Name ?? "ADMIN"
        //        };

        //        await _caseService.SavePrivatePartyAsync(entity);

        //        SessionHelper.Clear(HttpContext);

        //        return Json(new AjaxResponse
        //        {
        //            Success = true,
        //            Message = "Case Registration Completed Successfully"
        //        });

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error saving case registration step 1.");
        //        return Json(new AjaxResponse
        //        {
        //            Success = false,
        //            Message = SaveErrorMessage
        //        });
        //    }
        //}

        [HttpPost]
        public async Task<JsonResult> SaveFullCaseRegistration(CaseRegistrationWizardViewModel model)
        {
            try
            {
                var errors = ValidateFullCaseRegistration(model);

                if (errors.Any())
                {
                    return Json(new AjaxResponse
                    {
                        Success = false,
                        Message = string.Join("<br/>", errors)
                    });
                }

                long caseId = await _caseService.SaveFullCaseRegistrationAsync(
                    model,
                    User.Identity?.Name ?? "ADMIN");

                return Json(new AjaxResponse
                {
                    Success = true,
                    CaseId = caseId,
                    Message = "Case Registration Completed Successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving full case registration.");

                return Json(new AjaxResponse
                {
                    Success = false,
                    Message = SaveErrorMessage
                });
            }
        }

        private static List<string> ValidateFullCaseRegistration(CaseRegistrationWizardViewModel model)
        {
            var errors = new List<string>();

            if (model.InstitutionDate == null) errors.Add("Institution Date is required.");
            if (string.IsNullOrWhiteSpace(model.CaseNumber)) errors.Add("Case Number is required.");
            if (model.CaseTypeId == null) errors.Add("Case Type is required.");
            if (model.CaseSubjectId == null) errors.Add("Case Subject is required.");
            if (model.CasePurposeId == null) errors.Add("Case Purpose is required.");
            if (model.HearingDate == null) errors.Add("Hearing Date is required.");
            if (model.BenchTypeId == null) errors.Add("Bench Type is required.");
            if (string.IsNullOrWhiteSpace(model.AppellantName)) errors.Add("Appellant Name is required.");
            if (model.DesignationId == null) errors.Add("Appellant Designation is required.");
            if (model.DistrictId == null) errors.Add("Appellant District is required.");
            if (model.DepartmentId == null) errors.Add("Respondent Department is required.");

            if (!string.IsNullOrWhiteSpace(model.CaseNumber))
            {
                var parts = model.CaseNumber.Split('/');
                if (model.CaseNumber.Length != 10 || parts.Length != 2 || parts[0].Length != 5 || parts[1].Length != 4)
                    errors.Add("Please enter valid Case Number. Format should be 12345/2026.");
            }

            if (model.IsImpungned && model.DateofImpugnedOrder == null)
                errors.Add("Date of Impugned Order is required.");

            if (model.DateofImpugnedOrder > model.InstitutionDate)
                errors.Add("Date of Impugned Order cannot be greater than Institution Date.");

            if (model.InstitutionDate > model.HearingDate)
                errors.Add("Hearing Date must be greater than or equal to Institution Date.");

            return errors;
        }

        public async Task<IActionResult> StateList(int pageNo = 1, int rowCnt = 999999999)
        {
            var data =
                await _service.GetAllAsync(
                    pageNo,
                    rowCnt);

            ViewBag.PageNo = pageNo;
            ViewBag.RowCnt = rowCnt;

            return View(data);
        }
        private async Task BindDropdowns(CaseRegistrationWizardViewModel vm )
        {

            //Department
            var orders = await _RcsatDeptservice.GetDepartmentNameListAsync();
            ViewBag.OrderList = orders;

            var casetype = await _CaseTypeservice.GetCaseTypeAsync(1, 1000);
            ViewBag.CaseTypeList = casetype;

            
            var casesubject = await _CaseSubjectService.GetCaseSubjectAsync(1, 1000);
            ViewBag.CaseSubjectList = casesubject;

            var casepurpose = await _CasePurposeService.GetCasePurposeAsync(1, 1000);
            ViewBag.CasePurposeList = casepurpose;

            var Bench = await _BenchTypeService.GetBenchDDL(1, 1000);
            ViewBag.BenchList = Bench;

            var District = await _DistrictService.GetAllAsync(1, 1000);
            ViewBag.DistrictList = District;

            var Designation = await _DesignationService.GetDesignationDDL(1, 1000);
            ViewBag.DesignationList = Designation;

            var courtCode = HttpContext.Session.GetString("CourtCode") ?? "0";
          
            var AppleantAdvocate = await _AdvocateService.GetAdvocatesByCourtCodeAsync(courtCode.ToString());
            ViewBag.AppleantAdvocateList = AppleantAdvocate;

            var PrivateAdvocate = await _AdvocateService.GetPrivateAdvocatesAsync();
            ViewBag.PrivateAdvocateList = PrivateAdvocate;


        }

        [HttpGet]
        public async Task<IActionResult> GetRespondentAdvocates(long departmentId)
        {
            try
            {
                var courtCode = HttpContext.Session.GetString("CourtCode") ?? "0";

                if (departmentId <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Please select department."
                    });
                }

                // Respondent Advocates
                var respondentAdvocates =
                    await _AdvocateService.GetRespondentAdvocatesAsync(
                        courtCode,
                        departmentId
                    );

                return Json(new
                {
                    success = true,
                    data = respondentAdvocates
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading respondent advocates.");

                return Json(new
                {
                    success = false,
                    message = "Unable to load respondent advocates."
                });
            }
        }
    }
}