using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.Services
{
    public interface IPageService
    {
        Task<Page?> GetPageBySlugAsync(string slug);
        Task<Page?> GetPageByIdAsync(int id);
        Task<List<Page>> GetAllPagesAsync();
        Task<Page> CreatePageAsync(Page page);
        Task<Page> UpdatePageAsync(Page page);
        Task DeletePageAsync(int id);
    }

    public class PageService : IPageService
    {
        private readonly ApplicationDbContext _context;

        public PageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Page?> GetPageBySlugAsync(string slug)
        {
            return await _context.Pages
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsActive);
        }

        public async Task<Page?> GetPageByIdAsync(int id)
        {
            return await _context.Pages.FindAsync(id);
        }

        public async Task<List<Page>> GetAllPagesAsync()
        {
            return await _context.Pages
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Page> CreatePageAsync(Page page)
        {
            page.CreatedAt = DateTime.UtcNow;
            page.Slug = GenerateSlug(page.Title);
            
            _context.Pages.Add(page);
            await _context.SaveChangesAsync();
            return page;
        }

        public async Task<Page> UpdatePageAsync(Page page)
        {
            var existing = await _context.Pages.FindAsync(page.Id)
                ?? throw new KeyNotFoundException("Page not found");

            existing.Title = page.Title;
            existing.TitleBn = page.TitleBn;
            existing.Content = page.Content;
            existing.ContentBn = page.ContentBn;
            existing.MetaTitle = page.MetaTitle;
            existing.MetaDescription = page.MetaDescription;
            existing.MetaKeywords = page.MetaKeywords;
            existing.FeaturedImage = page.FeaturedImage;
            existing.BannerImage = page.BannerImage;
            existing.IsActive = page.IsActive;
            existing.ShowInFooter = page.ShowInFooter;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedBy = page.UpdatedBy;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeletePageAsync(int id)
        {
            var page = await _context.Pages.FindAsync(id)
                ?? throw new KeyNotFoundException("Page not found");

            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
        }

        private static string GenerateSlug(string title)
        {
            var slug = title.ToLower()
                .Replace(" ", "-")
                .Replace("&", "and");
            
            // Remove special characters
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");
            
            return slug.Trim('-');
        }
    }
}
