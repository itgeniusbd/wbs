using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace WBS.Web.Services
{
    public interface IAuthorizationService
    {
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string module, string action);
        Task<List<string>> GetUserPermissionsAsync(string userId);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthorizationService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string module, string action)
        {
            if (user == null || !user.Identity!.IsAuthenticated)
                return false;

            // Dashboard view is allowed for all authenticated users
            if (module == "Dashboard" && action == "View")
                return true;

            // Admin has all permissions
            if (user.IsInRole("Admin"))
                return true;

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return false;

            // Get role names from user claims
            var userRoleNames = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!userRoleNames.Any())
                return false;

            // Get role IDs from role names
            var roleIds = await _context.Roles
                .Where(r => userRoleNames.Contains(r.Name))
                .Select(r => r.Id)
                .ToListAsync();

            if (!roleIds.Any())
                return false;

            // Check if any of the user's roles have the required permission
            var hasPermission = await _context.RolePermissions
                .AnyAsync(rp => roleIds.Contains(rp.RoleId) &&
                               rp.Permission.Module == module &&
                               rp.Permission.Action == action);

            return hasPermission;
        }

        public async Task<List<string>> GetUserPermissionsAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return new List<string>();

            var userRoleIds = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            var permissions = await _context.RolePermissions
                .Where(rp => userRoleIds.Contains(rp.RoleId))
                .Select(rp => $"{rp.Permission.Module}.{rp.Permission.Action}")
                .Distinct()
                .ToListAsync();

            return permissions;
        }
    }
}
