using Microsoft.AspNetCore.Localization;

namespace WBS.Web.Services
{
    public interface ILanguageService
    {
        string GetCurrentLanguage(HttpContext httpContext);
        void SetLanguage(HttpContext httpContext, string culture);
        string GetMenuName(string nameEn, string? nameBn, string currentLanguage);
    }

    public class LanguageService : ILanguageService
    {
        public string GetCurrentLanguage(HttpContext httpContext)
        {
            var feature = httpContext.Features.Get<IRequestCultureFeature>();
            var culture = feature?.RequestCulture.Culture.TwoLetterISOLanguageName ?? "en";
            return culture;
        }

        public void SetLanguage(HttpContext httpContext, string culture)
        {
            httpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions 
                { 
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    Path = "/"
                }
            );
        }

        public string GetMenuName(string nameEn, string? nameBn, string currentLanguage)
        {
            if (currentLanguage == "bn" && !string.IsNullOrEmpty(nameBn))
                return nameBn;
            return nameEn;
        }
    }
}
