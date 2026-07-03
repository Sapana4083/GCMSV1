using GCMS.Models;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class UserMasterController : Controller
    {
        private readonly IRoleService _roleService;

        public UserMasterController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public IActionResult UserLogin()
        {
            return View();
        }
      
        public async Task<IActionResult> Roles(int pageNo = 1, int rowCnt = 9999999)
        {
            var data = await _roleService.GetAllAsync(pageNo, rowCnt);

            if (data == null)
                data = new List<RoleMaster>();

            ViewBag.PageNo = pageNo;
            ViewBag.RowCnt = rowCnt;

            return View(data);
        }



        [HttpGet]
        public async Task<JsonResult> GetRole(long id)
        {
            return Json(await _roleService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<JsonResult> SaveRole(RoleMaster model)
        {
            //if (ModelState.ContainsKey("OrderNo"))
            //    ModelState.Remove("OrderNo");

            //if (model.OrderNo == null || model.OrderNo <= 0)
            //    ModelState.AddModelError("OrderNo", "Order Number must be greater than 0.");

            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values
            //        .SelectMany(v => v.Errors)
            //        .Select(e => e.ErrorMessage)
            //        .FirstOrDefault();

            //    return Json(new { success = false, message = errors });
            //}

            try
            {
                if (model.RoleId == 0)
                {
                    model.CreateBy =
                        HttpContext.Session.GetString("Username") ?? "SYSTEM";

                    await _roleService.AddAsync(model);
                }
                else
                {
                    model.ModifyBy =
                        HttpContext.Session.GetString("Username") ?? "SYSTEM";

                    await _roleService.UpdateAsync(model);
                }

                return Json(new { success = true, message = "Saved Successfully" });
            }
            catch (InvalidOperationException ex)
            {
                // Catches duplicate role name raised by proc (-20001)
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }




        public IActionResult Menu()
        {
            return View();
        }
    }
}
