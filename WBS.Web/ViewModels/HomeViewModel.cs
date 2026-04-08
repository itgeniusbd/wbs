namespace WBS.Web.ViewModels
{
    public class HomeViewModel
    {
        public List<SliderViewModel> Sliders { get; set; } = new();
        public List<DonationTypeViewModel> DonationTypes { get; set; } = new();
        public List<AppealViewModel> FeaturedAppeals { get; set; } = new();
        public List<EventViewModel> FeaturedEvents { get; set; } = new();
        public List<NewsViewModel> LatestNews { get; set; } = new();
        public List<StoryViewModel> FeaturedStories { get; set; } = new();
        public List<SDGViewModel> SDGs { get; set; } = new();
        public List<ProgramViewModel> FeaturedPrograms { get; set; } = new();
        public List<VideoGalleryViewModel> FeaturedVideos { get; set; } = new();
        public List<PartnerViewModel> Partners { get; set; } = new();
        public SiteSettingsViewModel? SiteSettings { get; set; }
        public StatisticsViewModel Statistics { get; set; } = new();
    }

    public class StatisticsViewModel
    {
        public int TotalPrograms { get; set; }
        public int TotalEvents { get; set; }
        public int TotalDistricts { get; set; }
        public int TotalThanas { get; set; }
        public int TotalBeneficiaries { get; set; }
    }

    public class SliderViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? TitleBn { get; set; }
        public string? Subtitle { get; set; }
        public string? SubtitleBn { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? ButtonText { get; set; }
        public string? ButtonTextBn { get; set; }
        public string? ButtonUrl { get; set; }
        public string? SecondButtonText { get; set; }
        public string? SecondButtonUrl { get; set; }
    }

    public class DonationTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? NameBn { get; set; }
        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }
        public string? Icon { get; set; }
    }

    public class AppealViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? TitleBn { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? SummaryBn { get; set; }
        public string? FeaturedImage { get; set; }
        public decimal? TargetAmount { get; set; }
        public decimal RaisedAmount { get; set; }
        public bool IsUrgent { get; set; }
        public int DonationPercentage => TargetAmount.HasValue && TargetAmount > 0 
            ? (int)Math.Min(100, (RaisedAmount / TargetAmount.Value) * 100) 
            : 0;
    }

    public class NewsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? TitleBn { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? SummaryBn { get; set; }
        public string? FeaturedImage { get; set; }
        public DateTime PublishedAt { get; set; }
    }

    public class StoryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? TitleBn { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? BeneficiaryName { get; set; }
        public string? BeneficiaryNameBn { get; set; }
        public string? Summary { get; set; }
        public string? SummaryBn { get; set; }
        public string? FeaturedImage { get; set; }
        public string? VideoUrl { get; set; }
    }

    public class SDGViewModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? NameBn { get; set; }
        public string? Image { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
    }

    public class PartnerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? NameBn { get; set; }
        public string? Logo { get; set; }
        public string? Website { get; set; }
    }

    public class SiteSettingsViewModel
    {
        public string SiteName { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? FacebookUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? YouTubeUrl { get; set; }
    }

    public class ProgramViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? TitleBn { get; set; }
        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }
        public string? FeaturedImage { get; set; }
        public string SDGName { get; set; } = string.Empty;
        public string? SDGNameBn { get; set; }
        public string? SDGColor { get; set; }
        public int EventCount { get; set; }
    }

    public class EventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? TitleBn { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }
        public string? FeaturedImage { get; set; }
        public string? Location { get; set; }
        public string? LocationBn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? TicketPrice { get; set; }
        public int? TotalCapacity { get; set; }
        public int RegisteredCount { get; set; }
        public int AvailableSeats { get; set; }
    }

    public class VideoGalleryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? TitleBn { get; set; }
        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }
        public string YouTubeUrl { get; set; } = string.Empty;
        public string? YouTubeVideoId { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
