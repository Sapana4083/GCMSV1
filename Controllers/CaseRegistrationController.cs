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

namespace GCMS.Controllers
{
    public class CaseRegistrationController : Controller
    {
        private const string SaveErrorMessage = "Unable to save case registration details. Please try again or contact support.";
        private readonly ICaseService _caseService;
        private readonly IDepartmentService _service;
        private readonly ICaseTypeService _CaseTypeservice;
        private readonly ICaseSubjectService _CaseSubjectService;
        private readonly ICasePurposeService _CasePurposeService;
        private readonly IBenchTypeService _BenchTypeService;
        private readonly IDistrictService _DistrictService;
        private readonly IDesignationService _DesignationService;
        private readonly ILogger<CaseRegistrationController> _logger;
        

        public CaseRegistrationController(IDepartmentService service, ICaseService caseService, ICaseTypeService caseTypeService, ICaseSubjectService caseSubjectService,ICasePurposeService casePurposeService,IBenchTypeService benchTypeService,IDistrictService districtService,IDesignationService designationService,ILogger<CaseRegistrationController> logger)
        {
            _service = service;
            _caseService = caseService;
            _logger = logger;
            _CaseTypeservice = caseTypeService;
            _CaseSubjectService = caseSubjectService;
            _CasePurposeService = casePurposeService;
            _BenchTypeService = benchTypeService;
            _DistrictService = districtService;
            _DesignationService = designationService;
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
        [HttpPost]
        public async Task<JsonResult> SaveStep1(CaseRegistrationWizardViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new AjaxResponse
                    {
                        Success = false,
                        Message = string.Join("<br/>",
                        ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage))
                    });
                }

                var entity = new CaseRegistration
                {
                    InstitutionDate = model.InstitutionDate,
                    CaseNo = model.CaseNumber,
                    //ManualCaseNo = model.ManualCaseNumber,
                    OrderNo = model.OrderNumber,
                    DateOfOrder = model.DateofImpugnedOrder,
                    OrderIssuedById = model.OrderIssuedById,
                    CourtCode = model.CourtCode,
                    CaseTypeId = model.CaseTypeId,
                    CaseSubjectId = model.CaseSubjectId,
                    CasePurposeId = model.CasePurposeId,
                    HearingDate = model.HearingDate,
                    BenchTypeId = model.BenchTypeId,
                    LinkedCase = model.LinkedCaseNumber,
                    OldCaseNo = model.OldCaseNumber,
                    CreatedBy = User.Identity?.Name ?? "ADMIN"
                };
                long caseId = SessionHelper.GetCaseId(HttpContext);

                if (caseId == 0)
                {
                    caseId = await _caseService.SaveCaseAsync(entity);
                    SessionHelper.SetCaseId(HttpContext, caseId);
                }
                else
                {
                    entity.CaseId = caseId;
                    await _caseService.UpdateCaseAsync(entity);
                }

                //long caseId = await _caseService.SaveCaseAsync(entity);

                SessionHelper.SetCaseId(HttpContext, caseId);

                return Json(new AjaxResponse
                {
                    Success = true,
                    CaseId = caseId,
                    Message = "Step1 Saved Successfully"
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving case registration step 1.");
                return Json(new AjaxResponse
                {
                    Success = false,
                    Message = SaveErrorMessage
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveStep2(CaseRegistrationWizardViewModel model)
        {
            try
            {
                long caseId = SessionHelper.GetCaseId(HttpContext);

                if (caseId == 0)
                {
                    return Json(new AjaxResponse
                    {
                        Success = false,
                        Message = "Session Expired"
                    });
                }

                var entity = new CaseAppellant
                {
                    CaseId = caseId,
                    AppellantName = model.AppellantName,
                    Designation = model.DesignationId?.ToString(),
                    District = model.DistrictId?.ToString(),
                    MobileNo = model.MobileNumber,
                    EmployeeId = model.EmployeeId,
                    AdvocateId = model.AdvocateId,
                    AdvocateEmail = model.AdvocateEmail,
                    AdvocateMobile = model.AdvocateMobile?.ToString()
                };

                await _caseService.SaveAppellantAsync(entity);

                return Json(new AjaxResponse
                {
                    Success = true,
                    Message = "Step2 Saved Successfully"
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving case registration step 1.");
                return Json(new AjaxResponse
                {
                    Success = false,
                    Message = SaveErrorMessage
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveStep3(CaseRegistrationWizardViewModel model)
        {
            try
            {
                long caseId = SessionHelper.GetCaseId(HttpContext);

                if (caseId == 0)
                {
                    return Json(new AjaxResponse
                    {
                        Success = false,
                        Message = "Session Expired"
                    });
                }

                var entity = new CaseRespondent
                {
                    CaseId = caseId,
                    DepartmentId = model.DepartmentId,
                    AdvocateId = model.RespondentAdvocateId,
                    AdvocateEmail = model.RespondentAdvocateEmail,
                    AdvocateMobile = model.RespondentAdvocateMobile,
                    CreatedBy = "ADMIN"
                };

                await _caseService.SaveRespondentAsync(entity);

                return Json(new AjaxResponse
                {
                    Success = true,
                    Message = "Step3 Saved Successfully"
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving case registration step 1.");
                return Json(new AjaxResponse
                {
                    Success = false,
                    Message = SaveErrorMessage
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> SaveStep4(CaseRegistrationWizardViewModel model)
        {
            try
            {
                long caseId = SessionHelper.GetCaseId(HttpContext);

                var entity = new CasePrivateParty
                {
                    CaseId = caseId,
                    PartyName = model.PrivatePartyName,
                    Designation = model.PrivateDesignation,
                    Advocate = model.AdvocateId,
                    CreatedBy = User.Identity?.Name ?? "ADMIN"
                };

                await _caseService.SavePrivatePartyAsync(entity);

                SessionHelper.Clear(HttpContext);

                return Json(new AjaxResponse
                {
                    Success = true,
                    Message = "Case Registration Completed Successfully"
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving case registration step 1.");
                return Json(new AjaxResponse
                {
                    Success = false,
                    Message = SaveErrorMessage
                });
            }
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
            //vm.OrderIssuedByList = await _caseService.GetOrderTypesAsync();

            //vm.CaseTypes = await _caseService.GetCaseTypesAsync();

            //vm.CaseSubjects = await _caseService.GetCaseSubjectsAsync();

            //vm.CasePurposes = await _caseService.GetCasePurposesAsync();

            //vm.BenchTypes = await _caseService.GetBenchTypesAsync();

           // vm.Designations = await _caseService.GetDesignationsAsync();

            vm.Districts = await _caseService.GetDistrictsAsync();

            vm.Advocates = await _caseService.GetAdvocatesAsync();

            vm.Departments = await _caseService.GetDepartmentsAsync();

            //var states = await _service.GetAllAsync(1, 1000);
            //ViewBag.StateList = states;
            var orders = await _service.GetAllOrderAsync(1, 1000);
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
        }
    }
}