using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.ViewModels;

namespace WBS.Web.Services
{
    public interface IContentService
    {
        // Site Settings
        Task<SiteSettings?> GetSiteSettingsAsync();
        Task<SiteSettings> UpdateSiteSettingsAsync(SiteSettings settings);

        // Sliders
        Task<List<SliderViewModel>> GetActiveSlidersAsync();
        Task<List<Slider>> GetAllSlidersAsync();
        Task<Slider> CreateSliderAsync(Slider slider);
        Task<Slider> UpdateSliderAsync(Slider slider);
        Task DeleteSliderAsync(int id);

        // Appeals
        Task<List<AppealViewModel>> GetFeaturedAppealsAsync(int count = 6);
        Task<List<Appeal>> GetAllAppealsAsync();
        Task<Appeal?> GetAppealBySlugAsync(string slug);
        Task<Appeal> CreateAppealAsync(Appeal appeal);
        Task<Appeal> UpdateAppealAsync(Appeal appeal);

        // News
        Task<List<NewsViewModel>> GetLatestNewsAsync(int count = 6);
        Task<List<News>> GetAllNewsAsync();
        Task<News?> GetNewsBySlugAsync(string slug);
        Task<News> CreateNewsAsync(News news);
        Task<News> UpdateNewsAsync(News news);

        // Events
        Task<List<EventViewModel>> GetFeaturedEventsAsync(int count = 6);

        // Stories
        Task<List<StoryViewModel>> GetFeaturedStoriesAsync(int count = 6);
        Task<Story?> GetStoryBySlugAsync(string slug);

        // SDGs
        Task<List<SDGViewModel>> GetActiveSDGsAsync();
        
        // Programs
        Task<List<ProgramViewModel>> GetFeaturedProgramsAsync(int count = 6);
        Task<List<ProgramViewModel>> GetProgramsBySDGAsync(int sdgId);

        // Video Gallery
        Task<List<VideoGalleryViewModel>> GetFeaturedVideosAsync(int count = 6);

        // Partners
        Task<List<PartnerViewModel>> GetActivePartnersAsync();

        // Statistics
        Task<StatisticsViewModel> GetStatisticsAsync();
    }

    public class ContentService : IContentService
    {
        private readonly ApplicationDbContext _context;

        public ContentService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Site Settings
        public async Task<SiteSettings?> GetSiteSettingsAsync()
        {
            return await _context.SiteSettings.FirstOrDefaultAsync();
        }

        public async Task<SiteSettings> UpdateSiteSettingsAsync(SiteSettings settings)
        {
            var existing = await _context.SiteSettings.FirstOrDefaultAsync();
            if (existing == null)
            {
                _context.SiteSettings.Add(settings);
            }
            else
            {
                _context.Entry(existing).CurrentValues.SetValues(settings);
                existing.UpdatedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return settings;
        }
        #endregion

        #region Sliders
        public async Task<List<SliderViewModel>> GetActiveSlidersAsync()
        {
            return await _context.Sliders
                .Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .Select(s => new SliderViewModel
                {
                    Id = s.Id,
                    Title = s.Title,
                    TitleBn = s.TitleBn,
                    Subtitle = s.Subtitle,
                    SubtitleBn = s.SubtitleBn,
                    ImageUrl = s.ImageUrl,
                    ButtonText = s.ButtonText,
                    ButtonTextBn = s.ButtonTextBn,
                    ButtonUrl = s.ButtonUrl,
                    SecondButtonText = s.SecondButtonText,
                    SecondButtonUrl = s.SecondButtonUrl
                })
                .ToListAsync();
        }

        public async Task<List<Slider>> GetAllSlidersAsync()
        {
            return await _context.Sliders.OrderBy(s => s.DisplayOrder).ToListAsync();
        }

        public async Task<Slider> CreateSliderAsync(Slider slider)
        {
            slider.CreatedAt = DateTime.UtcNow;
            _context.Sliders.Add(slider);
            await _context.SaveChangesAsync();
            return slider;
        }

        public async Task<Slider> UpdateSliderAsync(Slider slider)
        {
            var existing = await _context.Sliders.FindAsync(slider.Id)
                ?? throw new KeyNotFoundException("Slider not found");

            _context.Entry(existing).CurrentValues.SetValues(slider);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteSliderAsync(int id)
        {
            var slider = await _context.Sliders.FindAsync(id)
                ?? throw new KeyNotFoundException("Slider not found");

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Appeals
        public async Task<List<AppealViewModel>> GetFeaturedAppealsAsync(int count = 6)
        {
            return await _context.Appeals
                .Where(a => a.IsActive && a.IsFeatured)
                .OrderByDescending(a => a.IsUrgent)
                .ThenByDescending(a => a.CreatedAt)
                .Take(count)
                .Select(a => new AppealViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    TitleBn = a.TitleBn,
                    Slug = a.Slug,
                    Summary = a.Summary,
                    SummaryBn = a.SummaryBn,
                    FeaturedImage = a.FeaturedImage,
                    TargetAmount = a.TargetAmount,
                    RaisedAmount = a.RaisedAmount,
                    IsUrgent = a.IsUrgent
                })
                .ToListAsync();
        }

        public async Task<List<Appeal>> GetAllAppealsAsync()
        {
            return await _context.Appeals.OrderByDescending(a => a.CreatedAt).ToListAsync();
        }

        public async Task<Appeal?> GetAppealBySlugAsync(string slug)
        {
            return await _context.Appeals.FirstOrDefaultAsync(a => a.Slug == slug && a.IsActive);
        }

        public async Task<Appeal> CreateAppealAsync(Appeal appeal)
        {
            appeal.CreatedAt = DateTime.UtcNow;
            _context.Appeals.Add(appeal);
            await _context.SaveChangesAsync();
            return appeal;
        }

        public async Task<Appeal> UpdateAppealAsync(Appeal appeal)
        {
            var existing = await _context.Appeals.FindAsync(appeal.Id)
                ?? throw new KeyNotFoundException("Appeal not found");

            _context.Entry(existing).CurrentValues.SetValues(appeal);
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existing;
        }
        #endregion

        #region News
        public async Task<List<NewsViewModel>> GetLatestNewsAsync(int count = 6)
        {
            try
            {
                return await _context.News
                    .Where(n => n.IsActive)
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(count)
                    .Select(n => new NewsViewModel
                    {
                        Id = n.Id,
                        Title = n.Title,
                        TitleBn = n.TitleBn,
                        Slug = n.Slug,
                        Summary = n.Summary,
                        SummaryBn = n.SummaryBn,
                        FeaturedImage = n.FeaturedImage,
                        PublishedAt = n.CreatedAt // Use CreatedAt instead of PublishedAt
                    })
                    .ToListAsync();
            }
            catch
            {
                // Return empty list if News table doesn't exist or has issues
                return new List<NewsViewModel>();
            }
        }

        public async Task<List<News>> GetAllNewsAsync()
        {
            try
            {
                return await _context.News.OrderByDescending(n => n.CreatedAt).ToListAsync();
            }
            catch
            {
                return new List<News>();
            }
        }

        public async Task<News?> GetNewsBySlugAsync(string slug)
        {
            return await _context.News.FirstOrDefaultAsync(n => n.Slug == slug && n.IsActive);
        }

        public async Task<News> CreateNewsAsync(News news)
        {
            news.CreatedAt = DateTime.UtcNow;
            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return news;
        }

        public async Task<News> UpdateNewsAsync(News news)
        {
            var existing = await _context.News.FindAsync(news.Id)
                ?? throw new KeyNotFoundException("News not found");

            _context.Entry(existing).CurrentValues.SetValues(news);
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existing;
        }
        #endregion

        #region Events
        public async Task<List<EventViewModel>> GetFeaturedEventsAsync(int count = 6)
        {
            try
            {
                return await _context.Events
                    .Include(e => e.Registrations)
                    .Where(e => e.IsActive && e.IsFeatured)
                    .OrderBy(e => e.StartDate)
                    .Take(count)
                    .Select(e => new EventViewModel
                    {
                        Id = e.Id,
                        Title = e.Title,
                        TitleBn = e.TitleBn,
                        Slug = e.Slug,
                        Description = e.Description,
                        DescriptionBn = e.DescriptionBn,
                        FeaturedImage = e.FeaturedImage,
                        Location = e.Location,
                        LocationBn = e.LocationBn,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        TicketPrice = e.TicketPrice,
                        TotalCapacity = e.TotalCapacity,
                        RegisteredCount = e.Registrations.Count(r => r.Status == "Confirmed"),
                        AvailableSeats = e.TotalCapacity.HasValue ? e.TotalCapacity.Value - e.Registrations.Count(r => r.Status == "Confirmed") : 0
                    })
                    .ToListAsync();
            }
            catch
            {
                return new List<EventViewModel>();
            }
        }
        #endregion

        #region Stories
        public async Task<List<StoryViewModel>> GetFeaturedStoriesAsync(int count = 6)
        {
            return await _context.Stories
                .Where(s => s.IsActive && s.IsFeatured)
                .OrderByDescending(s => s.CreatedAt)
                .Take(count)
                .Select(s => new StoryViewModel
                {
                    Id = s.Id,
                    Title = s.Title,
                    TitleBn = s.TitleBn,
                    Slug = s.Slug,
                    BeneficiaryName = s.BeneficiaryName,
                    BeneficiaryNameBn = s.BeneficiaryNameBn,
                    Summary = s.Summary,
                    SummaryBn = s.SummaryBn,
                    FeaturedImage = s.FeaturedImage,
                    VideoUrl = s.VideoUrl
                })
                .ToListAsync();
        }

        public async Task<Story?> GetStoryBySlugAsync(string slug)
        {
            return await _context.Stories.FirstOrDefaultAsync(s => s.Slug == slug && s.IsActive);
        }
        #endregion

        #region SDGs
        public async Task<List<SDGViewModel>> GetActiveSDGsAsync()
        {
            return await _context.SDGs
                .Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ThenBy(s => s.Number)
                .Select(s => new SDGViewModel
                {
                    Id = s.Id,
                    Number = s.Number,
                    Name = s.Name,
                    NameBn = s.NameBn,
                    Image = s.Image,
                    Icon = s.Icon,
                    Color = s.Color
                })
                .ToListAsync();
        }
        #endregion

        #region Programs
        public async Task<List<ProgramViewModel>> GetFeaturedProgramsAsync(int count = 6)
        {
            return await _context.SDGPrograms
                .Where(p => p.IsActive && p.IsFeatured)
                .Include(p => p.SDG)
                .Include(p => p.Events)
                .OrderBy(p => p.DisplayOrder)
                .Take(count)
                .Select(p => new ProgramViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    TitleBn = p.TitleBn,
                    Description = p.Description,
                    DescriptionBn = p.DescriptionBn,
                    FeaturedImage = p.FeaturedImage,
                    SDGName = p.SDG != null ? p.SDG.Name : "",
                    SDGNameBn = p.SDG != null ? p.SDG.NameBn : "",
                    SDGColor = p.SDG != null ? p.SDG.Color : "",
                    EventCount = p.Events.Count
                })
                .ToListAsync();
        }

        public async Task<List<ProgramViewModel>> GetProgramsBySDGAsync(int sdgId)
        {
            return await _context.SDGPrograms
                .Where(p => p.IsActive && p.SDGId == sdgId)
                .Include(p => p.SDG)
                .Include(p => p.Events)
                .OrderBy(p => p.DisplayOrder)
                .Select(p => new ProgramViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    TitleBn = p.TitleBn,
                    Description = p.Description,
                    DescriptionBn = p.DescriptionBn,
                    FeaturedImage = p.FeaturedImage,
                    SDGName = p.SDG != null ? p.SDG.Name : "",
                    SDGNameBn = p.SDG != null ? p.SDG.NameBn : "",
                    SDGColor = p.SDG != null ? p.SDG.Color : "",
                    EventCount = p.Events.Count
                })
                .ToListAsync();
        }
        #endregion

        #region Partners
        public async Task<List<PartnerViewModel>> GetActivePartnersAsync()
        {
            return await _context.Partners
                .Where(p => p.IsActive)
                .OrderBy(p => p.DisplayOrder)
                .Select(p => new PartnerViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    NameBn = p.NameBn,
                    Logo = p.Logo,
                    Website = p.Website
                })
                .ToListAsync();
        }
        #endregion

        #region Statistics
        public async Task<StatisticsViewModel> GetStatisticsAsync()
        {
            try
            {
                var statistics = new StatisticsViewModel();

                // Get total active programs
                statistics.TotalPrograms = await _context.SDGPrograms
                    .Where(p => p.IsActive)
                    .CountAsync();

                // Get all active events/projects
                var allProjects = await _context.SDGProjects
                    .Where(p => p.IsActive)
                    .ToListAsync();

                statistics.TotalEvents = allProjects.Count;

                // Count districts where HasWork is true
                statistics.TotalDistricts = await _context.Districts
                    .Where(d => d.HasWork)
                    .CountAsync();

                // Count upazilas where HasWork is true
                statistics.TotalThanas = await _context.Upazilas
                    .Where(u => u.HasWork)
                    .CountAsync();

                // Sum total beneficiaries
                statistics.TotalBeneficiaries = allProjects.Sum(p => p.BeneficiaryCount);

                return statistics;
            }
            catch
            {
                // Return default statistics if there's an error
                return new StatisticsViewModel();
            }
        }
        #endregion

        #region Video Gallery
        public async Task<List<VideoGalleryViewModel>> GetFeaturedVideosAsync(int count = 6)
        {
            try
            {
                return await _context.VideoGalleries
                    .Where(v => v.IsActive && v.IsFeatured)
                    .OrderBy(v => v.DisplayOrder)
                    .ThenByDescending(v => v.CreatedAt)
                    .Take(count)
                    .Select(v => new VideoGalleryViewModel
                    {
                        Id = v.Id,
                        Title = v.Title,
                        TitleBn = v.TitleBn,
                        Description = v.Description,
                        DescriptionBn = v.DescriptionBn,
                        YouTubeUrl = v.YouTubeUrl,
                        YouTubeVideoId = v.YouTubeVideoId,
                        ThumbnailUrl = v.ThumbnailUrl
                    })
                    .ToListAsync();
            }
            catch
            {
                return new List<VideoGalleryViewModel>();
            }
        }
        #endregion
    }
}
