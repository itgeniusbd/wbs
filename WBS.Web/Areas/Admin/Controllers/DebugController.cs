using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DebugController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DebugController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> UserPermissions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Content("No user ID found");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Content("User not found");
            }

            // Get user's roles
            var roles = await _userManager.GetRolesAsync(user);
            
            // Get role claims from current user
            var roleClaims = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            // Get role IDs
            var roleIds = await _context.Roles
                .Where(r => roles.Contains(r.Name))
                .Select(r => new { r.Id, r.Name })
                .ToListAsync();

            // Get permissions for these roles
            var permissions = await _context.RolePermissions
                .Include(rp => rp.Permission)
                .Include(rp => rp.Role)
                .Where(rp => roleIds.Select(r => r.Id).Contains(rp.RoleId))
                .Select(rp => new
                {
                    Role = rp.Role.Name,
                    Module = rp.Permission.Module,
                    Action = rp.Permission.Action,
                    PermissionName = rp.Permission.Name
                })
                .OrderBy(p => p.Module)
                .ThenBy(p => p.Action)
                .ToListAsync();

            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <title>User Permissions Debug</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body>
    <div class='container mt-5'>
        <h1>User Permissions Debug</h1>
        <hr>
        
        <div class='card mb-4'>
            <div class='card-header'>
                <h3>User Information</h3>
            </div>
            <div class='card-body'>
                <p><strong>User ID:</strong> {userId}</p>
                <p><strong>Email:</strong> {user.Email}</p>
                <p><strong>Name:</strong> {user.FirstName} {user.LastName}</p>
            </div>
        </div>

        <div class='card mb-4'>
            <div class='card-header'>
                <h3>Roles from UserManager</h3>
            </div>
            <div class='card-body'>
                <ul>
                    {string.Join("", roles.Select(r => $"<li>{r}</li>"))}
                </ul>
            </div>
        </div>

        <div class='card mb-4'>
            <div class='card-header'>
                <h3>Role Claims from ClaimsPrincipal</h3>
            </div>
            <div class='card-body'>
                <ul>
                    {string.Join("", roleClaims.Select(r => $"<li>{r}</li>"))}
                </ul>
            </div>
        </div>

        <div class='card mb-4'>
            <div class='card-header'>
                <h3>Role IDs</h3>
            </div>
            <div class='card-body'>
                <ul>
                    {string.Join("", roleIds.Select(r => $"<li>{r.Name} - {r.Id}</li>"))}
                </ul>
            </div>
        </div>

        <div class='card mb-4'>
            <div class='card-header'>
                <h3>Assigned Permissions ({permissions.Count})</h3>
            </div>
            <div class='card-body'>
                <table class='table table-striped'>
                    <thead>
                        <tr>
                            <th>Role</th>
                            <th>Module</th>
                            <th>Action</th>
                            <th>Permission Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        {string.Join("", permissions.Select(p => $@"
                        <tr>
                            <td>{p.Role}</td>
                            <td>{p.Module}</td>
                            <td>{p.Action}</td>
                            <td>{p.PermissionName}</td>
                        </tr>"))}
                    </tbody>
                </table>
            </div>
        </div>

        <a href='/Admin/Dashboard' class='btn btn-primary'>Back to Dashboard</a>
    </div>
</body>
</html>";

            return Content(html, "text/html");
        }
    }
}
