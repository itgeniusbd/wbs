using Microsoft.AspNetCore.Mvc;
using WBS.Web.Services;
using WBS.Web.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Localization;

namespace WBS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContentService _contentService;
        private readonly IDonationService _donationService;
        private readonly IMenuService _menuService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IContentService contentService,
            IDonationService donationService,
            IMenuService menuService,
            ILogger<HomeController> logger)
        {
            _contentService = contentService;
            _donationService = donationService;
            _menuService = menuService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel();

            try
            {
                _logger.LogInformation("Loading home page data...");
                
                model.Sliders = await _contentService.GetActiveSlidersAsync();
                _logger.LogInformation("Sliders loaded: {Count}", model.Sliders.Count);
                
                model.DonationTypes = await _donationService.GetActiveDonationTypesAsync();
                _logger.LogInformation("DonationTypes loaded: {Count}", model.DonationTypes.Count);
                
                model.FeaturedAppeals = await _contentService.GetFeaturedAppealsAsync();
                _logger.LogInformation("FeaturedAppeals loaded: {Count}", model.FeaturedAppeals.Count);
                
                // Load Featured Events instead of Latest News
                model.FeaturedEvents = await _contentService.GetFeaturedEventsAsync();
                _logger.LogInformation("FeaturedEvents loaded: {Count}", model.FeaturedEvents.Count);
                
                // Try to load stories, but don't fail if table doesn't have required columns
                try
                {
                    model.FeaturedStories = await _contentService.GetFeaturedStoriesAsync();
                    _logger.LogInformation("FeaturedStories loaded: {Count}", model.FeaturedStories.Count);
                }
                catch (Exception storiesEx)
                {
                    _logger.LogWarning(storiesEx, "Could not load featured stories - table may need migration");
                    model.FeaturedStories = new List<StoryViewModel>();
                }
                
                model.SDGs = await _contentService.GetActiveSDGsAsync();
                _logger.LogInformation("SDGs loaded: {Count}", model.SDGs.Count);
                
                // Get featured programs with detailed logging
                try
                {
                    model.FeaturedPrograms = await _contentService.GetFeaturedProgramsAsync();
                    _logger.LogInformation("Featured Programs loaded successfully: {Count}", model.FeaturedPrograms.Count);
                    
                    foreach (var program in model.FeaturedPrograms)
                    {
                        _logger.LogInformation("Program: {Id} - {Title}", program.Id, program.Title);
                    }
                }
                catch (Exception programEx)
                {
                    _logger.LogError(programEx, "Error loading featured programs specifically");
                    model.FeaturedPrograms = new List<ProgramViewModel>();
                }
                
                // Try to load featured videos
                try
                {
                    model.FeaturedVideos = await _contentService.GetFeaturedVideosAsync();
                    _logger.LogInformation("Featured Videos loaded: {Count}", model.FeaturedVideos.Count);
                }
                catch (Exception videosEx)
                {
                    _logger.LogWarning(videosEx, "Could not load featured videos");
                    model.FeaturedVideos = new List<VideoGalleryViewModel>();
                }
                
                // Get statistics - do this before partners to ensure it always runs
                try
                {
                    model.Statistics = await _contentService.GetStatisticsAsync();
                    _logger.LogInformation("Statistics loaded: Programs={Programs}, Events={Events}, Districts={Districts}, Thanas={Thanas}, Beneficiaries={Beneficiaries}", 
                        model.Statistics.TotalPrograms, 
                        model.Statistics.TotalEvents, 
                        model.Statistics.TotalDistricts, 
                        model.Statistics.TotalThanas, 
                        model.Statistics.TotalBeneficiaries);
                }
                catch (Exception statsEx)
                {
                    _logger.LogError(statsEx, "Error loading statistics");
                    model.Statistics = new StatisticsViewModel();
                }
                
                // Try to load partners
                try
                {
                    model.Partners = await _contentService.GetActivePartnersAsync();
                    _logger.LogInformation("Partners loaded: {Count}", model.Partners.Count);
                }
                catch (Exception partnersEx)
                {
                    _logger.LogWarning(partnersEx, "Could not load partners - table may need migration");
                    model.Partners = new List<PartnerViewModel>();
                }

                var settings = await _contentService.GetSiteSettingsAsync();
                if (settings != null)
                {
                    model.SiteSettings = new SiteSettingsViewModel
                    {
                        SiteName = settings.SiteName,
                        Logo = settings.Logo,
                        Email = settings.Email,
                        Phone = settings.Phone,
                        Address = settings.Address,
                        FacebookUrl = settings.FacebookUrl,
                        TwitterUrl = settings.TwitterUrl,
                        InstagramUrl = settings.InstagramUrl,
                        YouTubeUrl = settings.YouTubeUrl
                    };
                }
                
                _logger.LogInformation("Home page data loaded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading home page data");
                // Return model with whatever was loaded successfully
            }

            return View(model);
        }

        // Debug action to check menus
        public async Task<IActionResult> TestMenus()
        {
            var menus = await _menuService.GetActiveMenusAsync();
            return Json(new
            {
                MenuCount = menus.Count,
                Menus = menus.Select(m => new
                {
                    m.Id,
                    m.Name,
                    m.Url,
                    m.Icon,
                    SubMenuCount = m.SubMenus.Count,
                    SubMenus = m.SubMenus.Select(s => new { s.Id, s.Name, s.Url })
                })
            });
        }

        // Debug action to check featured programs
        public async Task<IActionResult> TestPrograms()
        {
            try
            {
                var programs = await _contentService.GetFeaturedProgramsAsync();
                return Json(new
                {
                    Success = true,
                    ProgramCount = programs.Count,
                    Programs = programs.Select(p => new
                    {
                        p.Id,
                        p.Title,
                        p.TitleBn,
                        p.SDGName,
                        p.EventCount,
                        p.FeaturedImage
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }

        // API endpoint to get programs by SDG ID
        [HttpGet]
        [Route("api/programs/by-sdg/{sdgId}")]
        public async Task<IActionResult> GetProgramsBySDG(int sdgId)
        {
            try
            {
                var programs = await _contentService.GetProgramsBySDGAsync(sdgId);
                return Json(programs.Select(p => new
                {
                    id = p.Id,
                    title = p.Title,
                    titleBn = p.TitleBn,
                    description = p.Description,
                    descriptionBn = p.DescriptionBn
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting programs for SDG {SDGId}", sdgId);
                return Json(new List<object>());
            }
        }

        // Debug endpoint to check statistics
        [HttpGet]
        [Route("api/debug/statistics")]
        public async Task<IActionResult> DebugStatistics()
        {
            try
            {
                var stats = await _contentService.GetStatisticsAsync();
                return Json(new
                {
                    Success = true,
                    Statistics = stats,
                    Message = "Statistics retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl ?? "/");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
