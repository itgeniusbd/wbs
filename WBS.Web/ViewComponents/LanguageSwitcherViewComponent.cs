using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;

namespace WBS.Web.ViewComponents
{
    public class LanguageSwitcherViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var feature = HttpContext.Features.Get<IRequestCultureFeature>();
            var currentCulture = feature?.RequestCulture.Culture.TwoLetterISOLanguageName ?? "en";
            var returnUrl = HttpContext.Request.Path;

            var model = new LanguageSwitcherViewModel
            {
                CurrentCulture = currentCulture,
                ReturnUrl = returnUrl,
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo { Code = "en", Name = "English", NativeName = "EN" },
                    new CultureInfo { Code = "bn", Name = "Bengali", NativeName = "?????" }
                }
            };

            return View(model);
        }
    }

    public class LanguageSwitcherViewModel
    {
        public string CurrentCulture { get; set; } = "en";
        public string ReturnUrl { get; set; } = "/";
        public List<CultureInfo> SupportedCultures { get; set; } = new();
    }

    public class CultureInfo
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NativeName { get; set; } = string.Empty;
    }
}
