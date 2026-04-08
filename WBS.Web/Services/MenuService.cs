using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.ViewModels;

namespace WBS.Web.Services
{
    public interface IMenuService
    {
        Task<List<MenuViewModel>> GetActiveMenusAsync(string? language = null);
        Task<List<Menu>> GetAllMenusAsync();
        Task<Menu?> GetMenuByIdAsync(int id);
        Task<Menu> CreateMenuAsync(MenuCreateViewModel model);
        Task<Menu> UpdateMenuAsync(int id, MenuCreateViewModel model);
        Task DeleteMenuAsync(int id);
    }

    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext _context;

        public MenuService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuViewModel>> GetActiveMenusAsync(string? language = null)
        {
            var menus = await _context.Menus
                .Where(m => m.IsActive && m.ParentMenuId == null)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();

            bool isBangla = language == "bn";

            var result = new List<MenuViewModel>();

            foreach (var menu in menus)
            {
                var subMenus = await _context.Menus
                    .Where(s => s.ParentMenuId == menu.Id && s.IsActive)
                    .OrderBy(s => s.DisplayOrder)
                    .ToListAsync();

                var menuViewModel = new MenuViewModel
                {
                    Id = menu.Id,
                    Name = isBangla && !string.IsNullOrEmpty(menu.NameBn) ? menu.NameBn : menu.Name,
                    NameEn = menu.Name,
                    NameBn = menu.NameBn,
                    Url = menu.Url,
                    PageId = menu.PageId,
                    Icon = menu.Icon,
                    CssClass = menu.CssClass,
                    IsExternal = menu.IsExternal,
                    SubMenus = subMenus.Select(s => new MenuViewModel
                    {
                        Id = s.Id,
                        Name = isBangla && !string.IsNullOrEmpty(s.NameBn) ? s.NameBn : s.Name,
                        NameEn = s.Name,
                        NameBn = s.NameBn,
                        Url = s.Url,
                        PageId = s.PageId,
                        Icon = s.Icon,
                        IsExternal = s.IsExternal
                    }).ToList()
                };

                result.Add(menuViewModel);
            }

            return result;
        }

        public async Task<List<Menu>> GetAllMenusAsync()
        {
            return await _context.Menus
                .Include(m => m.SubMenus)
                .Include(m => m.ParentMenu)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Menu?> GetMenuByIdAsync(int id)
        {
            return await _context.Menus
                .Include(m => m.SubMenus)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Menu> CreateMenuAsync(MenuCreateViewModel model)
        {
            var menu = new Menu
            {
                Name = model.Name,
                NameBn = model.NameBn,
                Url = model.Url,
                ParentMenuId = model.ParentMenuId,
                PageId = model.PageId,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                IsExternal = model.IsExternal,
                Icon = model.Icon,
                CssClass = model.CssClass,
                CreatedAt = DateTime.UtcNow
            };

            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
            return menu;
        }

        public async Task<Menu> UpdateMenuAsync(int id, MenuCreateViewModel model)
        {
            var menu = await _context.Menus.FindAsync(id)
                ?? throw new KeyNotFoundException("Menu not found");

            menu.Name = model.Name;
            menu.NameBn = model.NameBn;
            menu.Url = model.Url;
            menu.ParentMenuId = model.ParentMenuId;
            menu.PageId = model.PageId;
            menu.DisplayOrder = model.DisplayOrder;
            menu.IsActive = model.IsActive;
            menu.IsExternal = model.IsExternal;
            menu.Icon = model.Icon;
            menu.CssClass = model.CssClass;
            menu.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return menu;
        }

        public async Task DeleteMenuAsync(int id)
        {
            var menu = await _context.Menus.FindAsync(id)
                ?? throw new KeyNotFoundException("Menu not found");

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
        }
    }
}
