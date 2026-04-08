using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;

namespace WBS.Web.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Main Activities Index Page
        public async Task<IActionResult> Index()
        {
            // Get latest news
            ViewBag.News = await _context.News
                .Where(n => n.IsActive)
                .OrderByDescending(n => n.PublishedDate)
                .Take(6)
                .ToListAsync();

            // Get upcoming events
            ViewBag.Events = await _context.Events
                .Where(e => e.IsActive && e.StartDate >= DateTime.Now)
                .OrderBy(e => e.StartDate)
                .Take(6)
                .ToListAsync();

            return View();
        }

        // Publications (/activities/publication)
        [Route("activities/publication")]
        public async Task<IActionResult> Publication(string? type = null)
        {
            var query = _context.Publications.Where(p => p.IsActive);

            var publications = await query
                .OrderByDescending(p => p.PublishedDate)
                .ToListAsync();

            ViewBag.Type = type;
            return View(publications);
        }

        // Blogs
        public async Task<IActionResult> Blogs()
        {
            return await Publication("Blog");
        }

        // Annual Reports (/activities/annual-reports or /activities/annualreports)
        [Route("activities/annual-reports")]
        [Route("activities/annualreports")]
        public async Task<IActionResult> AnnualReports()
        {
            var reports = await _context.AnnualReports
                .Where(r => r.IsActive)
                .OrderByDescending(r => r.Year)
                .ToListAsync();

            return View(reports);
        }

        // Gallery (/activities/gallery)
        [Route("activities/gallery")]
        public async Task<IActionResult> Gallery(string tab = "photos")
        {
            // Load Photo Galleries
            var photoGalleries = await _context.Galleries
                .Include(g => g.Images.Where(i => i.IsActive))
                .Where(g => g.IsActive)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();

            // Load Video Galleries
            var videoGalleries = await _context.VideoGalleries
                .Where(v => v.IsActive)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();

            ViewBag.PhotoGalleries = photoGalleries;
            ViewBag.VideoGalleries = videoGalleries;
            ViewBag.ActiveTab = tab;

            return View();
        }

        // Gallery Details
        [Route("activities/gallery/{id}")]
        public async Task<IActionResult> GalleryDetails(int id)
        {
            var gallery = await _context.Galleries
                .Include(g => g.Images.Where(i => i.IsActive).OrderBy(i => i.DisplayOrder))
                .FirstOrDefaultAsync(g => g.Id == id && g.IsActive);

            if (gallery == null)
                return NotFound();

            return View(gallery);
        }

        // News (/activities/news)
        [Route("activities/news")]
        public async Task<IActionResult> News(int page = 1)
        {
            var pageSize = 12;
            var news = await _context.News
                .Where(n => n.IsActive)
                .OrderByDescending(n => n.PublishedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(await _context.News.CountAsync(n => n.IsActive) / (double)pageSize);

            return View(news);
        }

        // Stories (/activities/stories)
        [Route("activities/stories")]
        public async Task<IActionResult> Stories()
        {
            var stories = await _context.Stories
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return View(stories);
        }

        // Story Details
        [Route("activities/stories/{slug}")]
        public async Task<IActionResult> StoryDetails(string slug)
        {
            var story = await _context.Stories
                .FirstOrDefaultAsync(s => s.Slug == slug && s.IsActive);

            if (story == null)
                return NotFound();

            return View(story);
        }

        // Policies (/activities/policies)
        [Route("activities/policies")]
        public async Task<IActionResult> Policies()
        {
            var policies = await _context.Policies
                .Where(p => p.IsActive)
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();
            
            return View(policies);
        }

        // Archive (/activities/archive)
        [Route("activities/archive")]
        public async Task<IActionResult> Archive(int? year = null)
        {
            var currentYear = DateTime.Now.Year;
            year = year ?? currentYear - 1;

            ViewBag.SelectedYear = year;
            ViewBag.AvailableYears = Enumerable.Range(2020, currentYear - 2020 + 1).Reverse().ToList();

            // Get archived news for the selected year
            ViewBag.News = await _context.News
                .Where(n => n.IsActive && n.PublishedDate.Year == year)
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();

            // Get archived events for the selected year
            ViewBag.Events = await _context.Events
                .Where(e => e.IsActive && e.StartDate.Year == year)
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();

            return View();
        }
    }
}
