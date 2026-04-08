using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Attributes;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Services;
using WBS.Web.ViewModels;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class MenusController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly ApplicationDbContext _context;

        public MenusController(IMenuService menuService, ApplicationDbContext context)
        {
            _menuService = menuService;
            _context = context;
        }

        [Permission("Menus", "View")]
        public async Task<IActionResult> Index()
        {
            var menus = await _menuService.GetAllMenusAsync();
            return View(menus);
        }

        [Permission("Menus", "Create")]
        public async Task<IActionResult> Create()
        {
            await LoadViewBagData();
            return View(new MenuCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Menus", "Create")]
        public async Task<IActionResult> Create(MenuCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewBagData();
                return View(model);
            }

            await _menuService.CreateMenuAsync(model);
            TempData["Success"] = "Menu created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Permission("Menus", "Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var menu = await _menuService.GetMenuByIdAsync(id);
            if (menu == null)
                return NotFound();

            await LoadViewBagData();
            var model = new MenuCreateViewModel
            {
                Name = menu.Name,
                NameBn = menu.NameBn,
                Url = menu.Url,
                ParentMenuId = menu.ParentMenuId,
                PageId = menu.PageId,
                DisplayOrder = menu.DisplayOrder,
                IsActive = menu.IsActive,
                IsExternal = menu.IsExternal,
                Icon = menu.Icon,
                CssClass = menu.CssClass
            };

            ViewBag.MenuId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Menus", "Edit")]
        public async Task<IActionResult> Edit(int id, MenuCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewBagData();
                ViewBag.MenuId = id;
                return View(model);
            }

            await _menuService.UpdateMenuAsync(id, model);
            TempData["Success"] = "Menu updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Menus", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _menuService.DeleteMenuAsync(id);
            TempData["Success"] = "Menu deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadViewBagData()
        {
            ViewBag.ParentMenus = await _context.Menus
                .Where(m => m.ParentMenuId == null)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();

            ViewBag.Pages = await _context.Pages
                .Where(p => p.IsActive)
                .OrderBy(p => p.Title)
                .ToListAsync();
        }
    }
}
