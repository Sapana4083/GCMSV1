using GCMS.Models;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.Controllers
{
    public class UserMasterController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IMenuService _menuService;
        private readonly IRoleMenuMappingService _roleMenuService;
           
        
        public UserMasterController(
            IRoleService roleService,
            IMenuService menuService,
            IRoleMenuMappingService roleMenuService)
        {
            _roleService = roleService;
            _menuService = menuService;
            _roleMenuService = roleMenuService;
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



        //  public async Task<IActionResult> MenuList(
        //int pageNo = 1,
        //int rowCnt = 1000)
        //  {
        //      var menus = await _menuService.GetAllAsync(pageNo, rowCnt);

        //      var model = menus
        //          .Where(x => x.ParentId == 0)
        //          .Select(parent => new
        //          {
        //              ParentMenu = parent.MenuName,

        //              ChildMenus = menus
        //                  .Where(x => x.ParentId == parent.MenuId)
        //                  .Select(x => x.MenuName)
        //                  .ToList()
        //          })
        //          .ToList();

        //      ViewBag.PageNo = pageNo;
        //      ViewBag.RowCnt = rowCnt;

        //      ViewBag.ParentMenus = menus.Where(x => x.ParentId == 0).ToList();

        //      return View(model);
        //  }

        public async Task<IActionResult> Menu(int pageNo = 1, int rowCnt = 1000)
        {
            var menus = await _menuService.GetAllAsync(pageNo, rowCnt);

            ViewBag.ParentMenus = menus
                .Where(x => x.ParentId == 0)
                .ToList();

            return View(menus);
        }

        [HttpGet]
        public async Task<JsonResult> GetChildMenus(long parentId)
        {
            var menus = await _menuService.GetAllAsync(1, 1000);

            var childMenus = menus
                .Where(x => x.ParentId == parentId)
                .Select(x => new
                {
                    menuId = x.MenuId,
                    menuName = x.MenuName
                })
                .ToList();

            return Json(childMenus);
        }

        [HttpGet]
        public async Task<IActionResult> GetMenu(long id)
        {
            var data = await _menuService.GetByIdAsync(id);

            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMenu(MenuMaster model)
        {
            try
            {
                if (model.MenuId == 0)
                {
                    model.CreateBy =
                        HttpContext.Session.GetString("Username") ?? "SYSTEM";

                    model.IsActvFlag = 1;
                    model.IsDelFlag = 0;

                    await _menuService.AddAsync(model);
                }
                else
                {
                    model.ModifyBy =
                        HttpContext.Session.GetString("Username") ?? "SYSTEM";

                    await _menuService.UpdateAsync(model);
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

        //RoleMenuMapping
     
        public async Task<IActionResult> RoleMenuMappingList()
        {
            var menus = await _menuService.GetAllAsync(1, 1000);

            ViewBag.MenuList = menus;

            ViewBag.ParentMenus = menus
                .Where(x => x.ParentId == 0)
                .ToList();

            ViewBag.RoleList = await _roleService.GetAllAsync(1, 1000);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetRolePermissions(long roleId)
        {
            var data = await _roleMenuService.GetByRoleIdAsync(roleId);

            return Json(data);
        }



        [HttpGet]
        public async Task<JsonResult> GetRoleMenuMapping(long id)
        {
            var data = await _roleMenuService.GetByIdAsync(id);

            return Json(data);
        }


        [HttpPost]
        public async Task<IActionResult> SaveRoleMenuMapping([FromBody] SaveRolePermissionVM model)
        {
            try
            {
                var username = HttpContext.Session.GetString("Username") ?? "SYSTEM";

                foreach (var item in model.Permissions)
                {
                    var permission = new RoleMenuMapping
                    {
                        RoleId = model.RoleId,
                        MenuId = item.MenuId,
                        IsActvFlag = item.IsActvFlag,
                        IsDelFlag = item.IsDelFlag,
                        CreateBy = username,
                        ModifyBy = username
                    };

                    // Check whether Role + Menu already exists
                    var existing = await _roleMenuService.GetByRoleAndMenuAsync(
                        permission.RoleId,
                        permission.MenuId);

                    if (existing == null)
                    {
                        // INSERT
                        await _roleMenuService.AddAsync(permission);
                    }
                    else
                    {
                        // UPDATE
                        permission.RoleMenuId = existing.RoleMenuId;

                        await _roleMenuService.UpdateAsync(permission);
                    }
                }

                return Json(new
                {
                    success = true,
                    message = "Permission Saved Successfully."
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

    }
}
