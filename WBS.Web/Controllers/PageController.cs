using Microsoft.AspNetCore.Mvc;
using WBS.Web.Services;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;

namespace WBS.Web.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly ApplicationDbContext _context;
        private readonly IContentService _contentService;

        public PageController(IPageService pageService, ApplicationDbContext context, IContentService contentService)
        {
            _pageService = pageService;
            _context = context;
            _contentService = contentService;
        }

        [Route("page/{slug}")]
        public async Task<IActionResult> Index(string slug)
        {
            // Check if this is the refund policy page
            if (slug == "refund-policy" || slug == "refund-return-policy")
            {
                return await RefundPolicy();
            }

            var page = await _pageService.GetPageBySlugAsync(slug);
            if (page == null)
                return NotFound();

            return View(page);
        }

        [Route("page/refund-policy")]
        public async Task<IActionResult> RefundPolicy()
        {
            var policy = await _context.Policies
                .Where(p => p.IsActive && (p.Title.Contains("Refund") || p.Title.Contains("Return")))
                .FirstOrDefaultAsync();

            var siteSettings = await _contentService.GetSiteSettingsAsync();

            return View((policy, siteSettings));
        }
    }
}
