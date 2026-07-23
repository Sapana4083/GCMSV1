using GCMS.Models;
using GCMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCMS.ViewComponents;

public class SidebarViewComponent : ViewComponent
{
    private readonly IMenuService _menuService;

    public SidebarViewComponent(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var menus = await _menuService.GetAllAsync(1, 1000);

        var tree = BuildTree(menus, 0);

        return View(tree);
    }

    private List<MenuMaster> BuildTree(List<MenuMaster> menus, long parentId)
    {
        return menus
            .Where(x => (x.ParentId ?? 0) == parentId)
            .Select(x =>
            {
                x.Children = BuildTree(menus, x.MenuId);
                return x;
            })
            .ToList();
    }
}