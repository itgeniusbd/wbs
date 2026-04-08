using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Attributes;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.ViewModels;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RolesController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        // GET: Admin/Roles
        [Permission("Roles", "View")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleViewModels = new List<RoleListViewModel>();

            foreach (var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
                var permissionCount = await _context.RolePermissions
                    .CountAsync(rp => rp.RoleId == role.Id);

                roleViewModels.Add(new RoleListViewModel
                {
                    Id = role.Id,
                    Name = role.Name ?? "",
                    Description = null,
                    IsActive = true,
                    UserCount = usersInRole.Count,
                    PermissionCount = permissionCount
                });
            }

            return View(roleViewModels);
        }

        // GET: Admin/Roles/Create
        [Permission("Roles", "Create")]
        public async Task<IActionResult> Create()
        {
            var permissions = await _context.Permissions
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            var permissionGroups = permissions
                .GroupBy(p => p.Module)
                .Select(g => new PermissionGroupViewModel
                {
                    Module = g.Key,
                    Permissions = g.Select(p => new PermissionViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        NameBn = p.NameBn,
                        Action = p.Action,
                        IsGranted = false
                    }).ToList()
                }).ToList();

            ViewBag.PermissionGroups = permissionGroups;
            return View(new CreateRoleViewModel());
        }

        // POST: Admin/Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Roles", "Create")]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = model.Name
                };

                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    // Add permissions
                    if (model.SelectedPermissions != null && model.SelectedPermissions.Any())
                    {
                        var rolePermissions = model.SelectedPermissions.Select(permissionId => new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = permissionId
                        });

                        _context.RolePermissions.AddRange(rolePermissions);
                        await _context.SaveChangesAsync();
                    }

                    TempData["Success"] = "Role created successfully.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            var permissions = await _context.Permissions
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            var permissionGroups = permissions
                .GroupBy(p => p.Module)
                .Select(g => new PermissionGroupViewModel
                {
                    Module = g.Key,
                    Permissions = g.Select(p => new PermissionViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        NameBn = p.NameBn,
                        Action = p.Action,
                        IsGranted = model.SelectedPermissions?.Contains(p.Id) ?? false
                    }).ToList()
                }).ToList();

            ViewBag.PermissionGroups = permissionGroups;
            return View(model);
        }

        // GET: Admin/Roles/Edit/5
        [Permission("Roles", "Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            // Prevent editing Admin role
            if (role.Name == "Admin")
            {
                TempData["Error"] = "Cannot edit the Admin role.";
                return RedirectToAction(nameof(Index));
            }

            var rolePermissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == id)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                Name = role.Name ?? "",
                Description = null,
                IsActive = true,
                SelectedPermissions = rolePermissions
            };

            var permissions = await _context.Permissions
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            var permissionGroups = permissions
                .GroupBy(p => p.Module)
                .Select(g => new PermissionGroupViewModel
                {
                    Module = g.Key,
                    Permissions = g.Select(p => new PermissionViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        NameBn = p.NameBn,
                        Action = p.Action,
                        IsGranted = rolePermissions.Contains(p.Id)
                    }).ToList()
                }).ToList();

            ViewBag.PermissionGroups = permissionGroups;
            return View(model);
        }

        // POST: Admin/Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Roles", "Edit")]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role == null)
                    return NotFound();

                // Prevent editing Admin role
                if (role.Name == "Admin")
                {
                    TempData["Error"] = "Cannot edit the Admin role.";
                    return RedirectToAction(nameof(Index));
                }

                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    // Update permissions
                    var existingPermissions = await _context.RolePermissions
                        .Where(rp => rp.RoleId == model.Id)
                        .ToListAsync();

                    _context.RolePermissions.RemoveRange(existingPermissions);

                    if (model.SelectedPermissions != null && model.SelectedPermissions.Any())
                    {
                        var newPermissions = model.SelectedPermissions.Select(permissionId => new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = permissionId
                        });

                        _context.RolePermissions.AddRange(newPermissions);
                    }

                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Role updated successfully.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            var permissions = await _context.Permissions
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            var permissionGroups = permissions
                .GroupBy(p => p.Module)
                .Select(g => new PermissionGroupViewModel
                {
                    Module = g.Key,
                    Permissions = g.Select(p => new PermissionViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        NameBn = p.NameBn,
                        Action = p.Action,
                        IsGranted = model.SelectedPermissions?.Contains(p.Id) ?? false
                    }).ToList()
                }).ToList();

            ViewBag.PermissionGroups = permissionGroups;
            return View(model);
        }

        // POST: Admin/Roles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Roles", "Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            // Prevent deleting Admin role
            if (role.Name == "Admin")
            {
                TempData["Error"] = "Cannot delete the Admin role.";
                return RedirectToAction(nameof(Index));
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            if (usersInRole.Any())
            {
                TempData["Error"] = "Cannot delete role with assigned users.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                TempData["Success"] = "Role deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to delete role.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
