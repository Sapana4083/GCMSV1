using GCMS.Data;
using GCMS.Models;
using GCMS.Services;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace GCMS.Controllers
{
    public class MasterController : Controller
    {
        private readonly IStateService _service;
        private readonly IDistrictService _districtService;
        private readonly IDivisionService _divisionService;
        private readonly ITehsilService _tehsilService;
        private readonly ApplicationDbContext _context;
        private readonly ISdoService _sdoService;

        public MasterController(
     IStateService service,
     ApplicationDbContext context,
     IDistrictService districtService,
     IDivisionService divisionService,
     ITehsilService tehsilService,
     ISdoService sdoService)
        {
            _service = service;
            _context = context;
            _districtService = districtService;
            _divisionService = divisionService;
            _tehsilService = tehsilService;
            _sdoService = sdoService;
        }

        //State Master
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

        [HttpPost]
        public async Task<JsonResult> SaveState(StateMaster model)
        {
            try
            {
                if (model.StateMastId == 0)
                {
                    //var maxId = await _context.StateMasters
                    // .MaxAsync(x => (long?)x.StateMastId) ?? 0;

                    //model.StateMastId = maxId + 1;
                    //model.CreatedOn = DateTime.Now;

                    //model.InActive = "F";
                    var username = HttpContext.Session.GetString("Username") ?? "SYSTEM";
                    var userSSO = HttpContext.Session.GetString("UserSSO");
                    var axUserID = HttpContext.Session.GetString("AxUserID");
                    model.CreatedBy = username;


                    await _service.AddAsync(model);
                }
                else

                {

                    await _service.UpdateAsync(model);
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
        public async Task<JsonResult> GetState(long id)
        {
            var state =
                await _service.GetByIdAsync(id);

            return Json(state);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteState(long id)
        {
            var state = await _service.GetByIdAsync(id);

            if (state == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Record not found"
                });
            }

            await _service.DeleteAsync(id);

            return Json(new
            {
                success = true
            });
        }

        //District Master
        public async Task<IActionResult> DistrictList(
     int pageNo = 1,
     int rowCnt = 1000)
        {
            var model = await _districtService.GetAllAsync(pageNo, rowCnt);

            ViewBag.StateList =
                await _service.GetAllAsync(1, 1000);

            ViewBag.DivisionList =
                await _divisionService.GetAllAsync(1, 1000);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SaveDistrict(DistrictMaster model)
        {
            try
            {
                model.CreatedBy =
                    HttpContext.Session.GetString("Username") ?? "SYSTEM";

                model.InActive = "F";

                if (model.DistrictMastId == 0)
                {
                    await _districtService.AddAsync(model);
                }
                else
                {
                    await _districtService.UpdateAsync(model);
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
        public async Task<IActionResult> GetDistrict(long id)
        {
            var data =
                await _districtService.GetByIdAsync(id);

            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDistrict(long id)
        {
            await _districtService.DeleteAsync(id);

            return Json(new
            {
                success = true
            });
        }

        //Division Master
        public async Task<IActionResult> DivisionList(int pageNo = 1, int rowCnt = 999999999)
        {
            var data = await _divisionService.GetAllAsync(pageNo, rowCnt);
            ViewBag.PageNo = pageNo;
            ViewBag.RowCnt = rowCnt;
            ViewBag.StateList =
                await _service
                .GetAllAsync(1, 1000);

            return View(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetDivision(long id)
        {
            return Json(await _divisionService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> SaveDivision(DivisionMaster model)
        {
            try
            {
                var username =
                       HttpContext.Session.GetString("Username")
                       ?? "SYSTEM";

                var userSSO =
                    HttpContext.Session.GetString("UserSSO");

                var axUserID =
                    HttpContext.Session.GetString("AxUserID");
                if (model.DivisionMastId == 0)
                {


                    model.CreatedBy = username;
                    model.UserName = username;

                    await _divisionService.AddAsync(model);
                }
                else
                {
                    model.CreatedBy = username;
                    model.UserName = username;
                    await _divisionService.UpdateAsync(model);
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

        [HttpPost]
        public async Task<JsonResult> DeleteDivision(long id)
        {
            await _divisionService.DeleteAsync(id);

            return Json(new
            {
                success = true
            });
        }


        //Tehsil Master
        public async Task<IActionResult> TehsilList()
        {
            var data = await _tehsilService.GetAllAsync(1, 100);

            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetTehsil(long id)
        {
            var data = await _tehsilService.GetByIdAsync(id);

            if (data == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Record not found."
                });
            }

            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTehsil(TehsilMaster model)
        {
            try
            {
                if (model.TehsilMastId == 0)
                {
                    model.CreatedBy = HttpContext.Session.GetString("Username") ?? "SYSTEM";

                    await _tehsilService.AddAsync(model);
                }
                else
                {
                    model.Username = HttpContext.Session.GetString("Username") ?? "SYSTEM";

                    await _tehsilService.UpdateAsync(model);
                }

                return Json(new
                {
                    success = true,
                    message = "Saved Successfully."
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
        public async Task<IActionResult> GetStates()
        {
            var data = await _service.GetAllAsync(1, 9999999);

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetDivisions()
        {
            var data = await _divisionService.GetAllAsync(1, 9999999);

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetDistricts(long stateId)
        {
            var data = await _districtService.GetAllAsync(1, 999999);

            return Json(data);
        }


        //SDO Master (Sub District Office)

        public async Task<IActionResult> SdoList()
        {
            var list = await _sdoService.GetAllAsync(1, 999999);

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSdo(SdoMaster model)
        {
            if (model.SdoMastId == 0)
                await _sdoService.AddAsync(model);
            else
                await _sdoService.UpdateAsync(model);

            return Json(new
            {
                success = true,
                message = "Saved Successfully"
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetSdo(long id)
        {
            var data = await _sdoService.GetByIdAsync(id);

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetSdoList(int pageNo = 1, int rowCount = 999999)
        {
            var data = await _sdoService.GetAllAsync(pageNo, rowCount);

            return Json(data);
        }
    }
}
