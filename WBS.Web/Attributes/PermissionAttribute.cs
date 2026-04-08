using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WBS.Web.Services;

namespace WBS.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(string module, string action)
            : base(typeof(PermissionFilter))
        {
            Arguments = new object[] { module, action };
        }
    }

    public class PermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly string _module;
        private readonly string _action;
        private readonly IAuthorizationService _authorizationService;

        public PermissionFilter(string module, string action, IAuthorizationService authorizationService)
        {
            _module = module;
            _action = action;
            _authorizationService = authorizationService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity!.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
                return;
            }

            var hasPermission = await _authorizationService.HasPermissionAsync(user, _module, _action);

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
