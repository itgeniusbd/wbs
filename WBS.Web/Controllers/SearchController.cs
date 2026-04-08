using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using Microsoft.AspNetCore.Localization;

namespace WBS.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string q, int page = 1, int pageSize = 12)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return View(new SearchResultViewModel
                {
                    Query = string.Empty,
                    TotalResults = 0,
                    CurrentPage = 1,
                    TotalPages = 0,
                    Results = new List<SearchResult>()
                });
            }

            var currentCulture = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.TwoLetterISOLanguageName ?? "en";
            bool isBangla = currentCulture == "bn";

            var results = new List<SearchResult>();
            var searchTerm = q.ToLower();

            // Search News
            var news = await _context.News
                .Where(n => n.IsActive && 
                    (n.Title.ToLower().Contains(searchTerm) || 
                     n.Content.ToLower().Contains(searchTerm) ||
                     (n.TitleBn != null && n.TitleBn.Contains(q)) ||
                     (n.ContentBn != null && n.ContentBn.Contains(q))))
                .ToListAsync();

            foreach (var item in news)
            {
                var title = isBangla && !string.IsNullOrEmpty(item.TitleBn) ? item.TitleBn : item.Title;
                var content = isBangla && !string.IsNullOrEmpty(item.ContentBn) ? item.ContentBn : item.Content;
                results.Add(new SearchResult
                {
                    Title = title,
                    Description = content?.Substring(0, Math.Min(200, content.Length)) + "...",
                    Url = $"/news/{item.Slug}",
                    Type = isBangla ? "?????" : "News",
                    Image = item.FeaturedImage,
                    Date = item.PublishedDate
                });
            }

            // Search Appeals
            var appeals = await _context.Appeals
                .Where(a => a.IsActive && 
                    (a.Title.ToLower().Contains(searchTerm) || 
                     (a.Summary != null && a.Summary.ToLower().Contains(searchTerm)) ||
                     (a.Content != null && a.Content.ToLower().Contains(searchTerm)) ||
                     (a.TitleBn != null && a.TitleBn.Contains(q)) ||
                     (a.SummaryBn != null && a.SummaryBn.Contains(q)) ||
                     (a.ContentBn != null && a.ContentBn.Contains(q))))
                .ToListAsync();

            foreach (var item in appeals)
            {
                var title = isBangla && !string.IsNullOrEmpty(item.TitleBn) ? item.TitleBn : item.Title;
                var summary = isBangla && !string.IsNullOrEmpty(item.SummaryBn) ? item.SummaryBn : item.Summary;
                results.Add(new SearchResult
                {
                    Title = title,
                    Description = summary?.Substring(0, Math.Min(200, summary.Length)) + "...",
                    Url = $"/appeals/{item.Slug}",
                    Type = isBangla ? "?????" : "Appeal",
                    Image = item.FeaturedImage,
                    Date = item.CreatedAt
                });
            }

            // Search History/About WBS
            var histories = await _context.Histories
                .Where(h => h.IsActive && 
                    (h.Title.ToLower().Contains(searchTerm) || 
                     h.Content.ToLower().Contains(searchTerm) ||
                     (h.TitleBn != null && h.TitleBn.Contains(q)) ||
                     (h.ContentBn != null && h.ContentBn.Contains(q))))
                .ToListAsync();

            foreach (var item in histories)
            {
                var title = isBangla && !string.IsNullOrEmpty(item.TitleBn) ? item.TitleBn : item.Title;
                var content = isBangla && !string.IsNullOrEmpty(item.ContentBn) ? item.ContentBn : item.Content;
                results.Add(new SearchResult
                {
                    Title = title,
                    Description = content?.Substring(0, Math.Min(200, content.Length)) + "...",
                    Url = "/about/history",
                    Type = isBangla ? "??????" : "History",
                    Image = item.FeaturedImage,
                    Date = item.CreatedAt
                });
            }

            // Search Annual Reports
            var annualReports = await _context.AnnualReports
                .Where(a => a.IsActive && 
                    (a.Title.ToLower().Contains(searchTerm) || 
                     (a.Description != null && a.Description.ToLower().Contains(searchTerm)) ||
                     (a.TitleBn != null && a.TitleBn.Contains(q)) ||
                     (a.DescriptionBn != null && a.DescriptionBn.Contains(q))))
                .ToListAsync();

            foreach (var item in annualReports)
            {
                var title = isBangla && !string.IsNullOrEmpty(item.TitleBn) ? item.TitleBn : item.Title;
                var description = isBangla && !string.IsNullOrEmpty(item.DescriptionBn) ? item.DescriptionBn : item.Description;
                results.Add(new SearchResult
                {
                    Title = title,
                    Description = description?.Substring(0, Math.Min(200, description.Length)) + "...",
                    Url = "/activities/annualreports",
                    Type = isBangla ? "??????? ?????????" : "Annual Report",
                    Image = item.CoverImage,
                    Date = item.CreatedAt
                });
            }

            // Search Publications
            var publications = await _context.Publications
                .Where(p => p.IsActive && 
                    (p.Title.ToLower().Contains(searchTerm) || 
                     (p.Description != null && p.Description.ToLower().Contains(searchTerm)) ||
                     (p.Author != null && p.Author.ToLower().Contains(searchTerm)) ||
                     (p.TitleBn != null && p.TitleBn.Contains(q)) ||
                     (p.DescriptionBn != null && p.DescriptionBn.Contains(q)) ||
                     (p.AuthorBn != null && p.AuthorBn.Contains(q))))
                .ToListAsync();

            foreach (var item in publications)
            {
                var title = isBangla && !string.IsNullOrEmpty(item.TitleBn) ? item.TitleBn : item.Title;
                var description = isBangla && !string.IsNullOrEmpty(item.DescriptionBn) ? item.DescriptionBn : item.Description;
                results.Add(new SearchResult
                {
                    Title = title,
                    Description = description?.Substring(0, Math.Min(200, description.Length)) + "...",
                    Url = "/activities/publication",
                    Type = isBangla ? "????????" : "Publication",
                    Image = item.CoverImage,
                    Date = item.PublishedDate
                });
            }

            // Search Gallery
            var galleries = await _context.Galleries
                .Where(g => g.IsActive && 
                    (g.Title.ToLower().Contains(searchTerm) || 
                     (g.Description != null && g.Description.ToLower().Contains(searchTerm)) ||
                     (g.TitleBn != null && g.TitleBn.Contains(q)) ||
                     (g.DescriptionBn != null && g.DescriptionBn.Contains(q))))
                .ToListAsync();

            foreach (var item in galleries)
            {
                var title = isBangla && !string.IsNullOrEmpty(item.TitleBn) ? item.TitleBn : item.Title;
                var description = isBangla && !string.IsNullOrEmpty(item.DescriptionBn) ? item.DescriptionBn : item.Description;
                results.Add(new SearchResult
                {
                    Title = title,
                    Description = description?.Substring(0, Math.Min(200, description.Length)) + "...",
                    Url = $"/activities/gallery/{item.Id}",
                    Type = isBangla ? "????????" : "Gallery",
                    Image = item.CoverImage,
                    Date = item.CreatedAt
                });
            }

            // Search Partners
            var partners = await _context.Partners
                .Where(p => p.IsActive && 
                    (p.Name.ToLower().Contains(searchTerm) ||
                     (p.NameBn != null && p.NameBn.Contains(q))))
                .ToListAsync();

            foreach (var item in partners)
            {
                var name = isBangla && !string.IsNullOrEmpty(item.NameBn) ? item.NameBn : item.Name;
                results.Add(new SearchResult
                {
                    Title = name,
                    Description = isBangla ? "?????? ???????" : "Our Partner",
                    Url = "/about/partners",
                    Type = isBangla ? "???????" : "Partner",
                    Image = item.Logo
                });
            }

            // Search Careers
            var careers = await _context.Careers
                .Where(c => c.IsActive && 
                    (c.Title.ToLower().Contains(searchTerm) || 
                     (c.Department != null && c.Department.ToLower().Contains(searchTerm)) ||
                     (c.Location != null && c.Location.ToLower().Contains(searchTerm)) ||
                     (c.Description != null && c.Description.ToLower().Contains(searchTerm)) ||
                     (c.TitleBn != null && c.TitleBn.Contains(q)) ||
                     (c.DescriptionBn != null && c.DescriptionBn.Contains(q))))
                .ToListAsync();

            foreach (var item in careers)
            {
                var title = isBangla && !string.IsNullOrEmpty(item.TitleBn) ? item.TitleBn : item.Title;
                var description = isBangla && !string.IsNullOrEmpty(item.DescriptionBn) ? item.DescriptionBn : item.Description;
                results.Add(new SearchResult
                {
                    Title = title,
                    Description = description?.Substring(0, Math.Min(200, description.Length)) + "...",
                    Url = $"/getinvolved/career/{item.Slug}",
                    Type = isBangla ? "??????????" : "Career",
                    Date = item.CreatedAt
                });
            }

            // Search Events
            var events = await _context.Events
                .Where(e => e.IsActive && 
                    (e.Title.ToLower().Contains(searchTerm) || 
                     e.Description.ToLower().Contains(searchTerm) ||
                     (e.TitleBn != null && e.TitleBn.Contains(q)) ||
                     (e.DescriptionBn != null && e.DescriptionBn.Contains(q))))
                .ToListAsync();

            foreach (var item in events)
            {
                var title = isBangla && !string.IsNullOrEmpty(item.TitleBn) ? item.TitleBn : item.Title;
                var description = isBangla && !string.IsNullOrEmpty(item.DescriptionBn) ? item.DescriptionBn : item.Description;
                results.Add(new SearchResult
                {
                    Title = title,
                    Description = description?.Substring(0, Math.Min(200, description.Length)) + "...",
                    Url = $"/ourwork/event/{item.Slug}",
                    Type = isBangla ? "??????" : "Event",
                    Image = item.FeaturedImage,
                    Date = item.StartDate
                });
            }

            // Search Stories
            var stories = await _context.Stories
                .Where(s => s.IsActive && 
                    (s.Title.ToLower().Contains(searchTerm) || 
                     s.Content.ToLower().Contains(searchTerm) ||
                     (s.TitleBn != null && s.TitleBn.Contains(q)) ||
                     (s.ContentBn != null && s.ContentBn.Contains(q))))
                .ToListAsync();

            foreach (var item in stories)
            {
                var title = isBangla && !string.IsNullOrEmpty(item.TitleBn) ? item.TitleBn : item.Title;
                var content = isBangla && !string.IsNullOrEmpty(item.ContentBn) ? item.ContentBn : item.Content;
                var publishedDate = item.CreatedAt;
                results.Add(new SearchResult
                {
                    Title = title,
                    Description = content?.Substring(0, Math.Min(200, content.Length)) + "...",
                    Url = $"/activities/stories/{item.Slug}",
                    Type = isBangla ? "??? ????" : "Story",
                    Image = item.FeaturedImage,
                    Date = publishedDate
                });
            }

            // Search Pages
            var pages = await _context.Pages
                .Where(p => p.IsActive && 
                    (p.Title.ToLower().Contains(searchTerm) || 
                     p.Content.ToLower().Contains(searchTerm) ||
                     (p.TitleBn != null && p.TitleBn.Contains(q)) ||
                     (p.ContentBn != null && p.ContentBn.Contains(q))))
                .ToListAsync();

            foreach (var item in pages)
            {
                var title = isBangla && !string.IsNullOrEmpty(item.TitleBn) ? item.TitleBn : item.Title;
                var content = isBangla && !string.IsNullOrEmpty(item.ContentBn) ? item.ContentBn : item.Content;
                results.Add(new SearchResult
                {
                    Title = title,
                    Description = content?.Substring(0, Math.Min(200, content.Length)) + "...",
                    Url = $"/page/{item.Slug}",
                    Type = isBangla ? "??????" : "Page",
                    Date = item.UpdatedAt ?? item.CreatedAt
                });
            }

            // Search SDG Programs
            var programs = await _context.SDGPrograms
                .Where(p => p.IsActive && 
                    (p.Title.ToLower().Contains(searchTerm) || 
                     p.Description.ToLower().Contains(searchTerm) ||
                     (p.TitleBn != null && p.TitleBn.Contains(q)) ||
                     (p.DescriptionBn != null && p.DescriptionBn.Contains(q))))
                .ToListAsync();

            foreach (var item in programs)
            {
                var title = isBangla && !string.IsNullOrEmpty(item.TitleBn) ? item.TitleBn : item.Title;
                var description = isBangla && !string.IsNullOrEmpty(item.DescriptionBn) ? item.DescriptionBn : item.Description;
                results.Add(new SearchResult
                {
                    Title = title,
                    Description = description?.Substring(0, Math.Min(200, description.Length)) + "...",
                    Url = $"/ourwork/sdg/{item.SDGId}/program/{item.Id}",
                    Type = isBangla ? "?????????" : "Program",
                    Image = item.FeaturedImage
                });
            }

            // Pagination
            var totalResults = results.Count;
            var totalPages = (int)Math.Ceiling(totalResults / (double)pageSize);
            var paginatedResults = results
                .OrderByDescending(r => r.Date ?? DateTime.MinValue)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new SearchResultViewModel
            {
                Query = q,
                TotalResults = totalResults,
                CurrentPage = page,
                TotalPages = totalPages,
                Results = paginatedResults
            };

            return View(viewModel);
        }
    }

    public class SearchResultViewModel
    {
        public string Query { get; set; } = string.Empty;
        public int TotalResults { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<SearchResult> Results { get; set; } = new();
    }

    public class SearchResult
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Image { get; set; }
        public DateTime? Date { get; set; }
    }
}
