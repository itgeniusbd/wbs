using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;

namespace WBS.Web.Controllers
{
    public class OurWorkController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OurWorkController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sdgs = await _context.SDGs
                .Include(s => s.Sectors.Where(sec => sec.IsActive))
                .Where(s => s.IsActive)
                .OrderBy(s => s.Number)
                .ToListAsync();

            return View(sdgs);
        }

        // SDG Page
        public async Task<IActionResult> SDG()
        {
            var sdgs = await _context.SDGs
                .Include(s => s.Sectors.Where(sec => sec.IsActive))
                .Where(s => s.IsActive)
                .OrderBy(s => s.Number)
                .ToListAsync();

            // Calculate statistics for each SDG
            var sdgStats = new Dictionary<int, dynamic>();
            foreach (var sdg in sdgs)
            {
                var programs = await _context.SDGPrograms
                    .Where(p => p.SDGId == sdg.Id && p.IsActive)
                    .CountAsync();

                var events = await _context.SDGProjects
                    .Where(p => p.SDGId == sdg.Id && p.IsActive)
                    .ToListAsync();

                sdgStats[sdg.Id] = new
                {
                    ProgramCount = programs,
                    EventCount = events.Count,
                    BeneficiaryCount = events.Sum(e => e.BeneficiaryCount)
                };
            }

            ViewBag.SDGStats = sdgStats;

            return View(sdgs);
        }

        // SDG Details - Show Programs
        [Route("ourwork/sdg/{id}")]
        public async Task<IActionResult> SDGDetails(int id)
        {
            var sdg = await _context.SDGs
                .Include(s => s.Sectors.Where(sec => sec.IsActive))
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

            if (sdg == null)
                return NotFound();

            // Get programs for this SDG
            var programs = await _context.SDGPrograms
                .Include(p => p.Events)
                .Where(p => p.SDGId == id && p.IsActive)
                .OrderBy(p => p.DisplayOrder)
                .ThenByDescending(p => p.CreatedAt)
                .ToListAsync();

            // Calculate statistics
            var allEvents = await _context.SDGProjects
                .Where(p => p.SDGId == id && p.IsActive)
                .ToListAsync();

            var stats = new
            {
                TotalPrograms = programs.Count,
                TotalEvents = allEvents.Count,
                TotalBeneficiaries = allEvents.Sum(p => p.BeneficiaryCount),
                Districts = allEvents.Where(p => !string.IsNullOrEmpty(p.District))
                    .Select(p => p.District).Distinct().Count(),
                Thanas = allEvents.Where(p => !string.IsNullOrEmpty(p.Thana))
                    .Select(p => p.Thana).Distinct().Count(),
                Unions = allEvents.Where(p => !string.IsNullOrEmpty(p.Union))
                    .Select(p => p.Union).Distinct().Count(),
                Villages = allEvents.Where(p => !string.IsNullOrEmpty(p.Village))
                    .Select(p => p.Village).Distinct().Count()
            };

            ViewBag.Programs = programs;
            ViewBag.Stats = stats;

            return View(sdg);
        }

        // Program Details - Show Events
        [Route("ourwork/program/{id}")]
        public async Task<IActionResult> ProgramDetails(int id)
        {
            var program = await _context.SDGPrograms
                .Include(p => p.SDG)
                .Include(p => p.Events)
                    .ThenInclude(e => e.Images)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (program == null)
                return NotFound();

            // Get events for this program
            var events = await _context.SDGProjects
                .Include(e => e.Images)
                .Where(e => e.SDGProgramId == id && e.IsActive)
                .OrderBy(e => e.DisplayOrder)
                .ThenByDescending(e => e.CreatedAt)
                .ToListAsync();

            // Calculate statistics
            var stats = new
            {
                TotalEvents = events.Count,
                TotalBeneficiaries = events.Sum(e => e.BeneficiaryCount),
                Districts = events.Where(e => !string.IsNullOrEmpty(e.District))
                    .Select(e => e.District).Distinct().Count()
            };

            ViewBag.Events = events;
            ViewBag.Stats = stats;

            return View(program);
        }

        // Event Details - Show Event Information
        [Route("ourwork/event/{id}")]
        public async Task<IActionResult> EventDetails(int id)
        {
            // Disable caching for this response
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            
            // Ensure UTF-8 encoding
            Response.ContentType = "text/html; charset=utf-8";
            
            var eventItem = await _context.SDGProjects
                .Include(e => e.SDG)
                .Include(e => e.SDGProgram)
                .Include(e => e.Images.OrderBy(i => i.DisplayOrder))
                .AsNoTracking() // Bypass EF cache
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // Sectors
        public async Task<IActionResult> Sectors()
        {
            var sectors = await _context.Sectors
                .Include(s => s.SDG)
                .Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();

            return View(sectors);
        }

        // Rohingya Page
        public async Task<IActionResult> Rohingya()
        {
            // Ensure UTF-8 encoding
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            Response.ContentType = "text/html; charset=utf-8";
            
            // Get Site Settings for manual statistics
            var siteSettings = await _context.SiteSettings.FirstOrDefaultAsync();
            
            // Get Rohingya Programs
            var rohingyaPrograms = await _context.SDGPrograms
                .Include(p => p.SDG)
                .Include(p => p.Events)
                .Where(p => p.IsActive && p.IsRohingyaProgram)
                .OrderBy(p => p.DisplayOrder)
                .ThenByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync();

            // Get Rohingya Events (both from programs and standalone events)
            var programIds = rohingyaPrograms.Select(p => p.Id).ToList();
            var rohingyaEvents = await _context.SDGProjects
                .Include(e => e.SDG)
                .Include(e => e.SDGProgram)
                .Include(e => e.Images)
                .Where(e => e.IsActive && (e.IsRohingyaEvent || (e.SDGProgramId.HasValue && programIds.Contains(e.SDGProgramId.Value))))
                .OrderBy(e => e.DisplayOrder)
                .ThenByDescending(e => e.CreatedAt)
                .AsNoTracking()
                .ToListAsync();

            // Get Rohingya Appeals
            var rohingyaAppeals = await _context.Appeals
                .Where(a => a.IsActive && a.Title.Contains("Rohingya"))
                .OrderByDescending(a => a.CreatedAt)
                .Take(10)
                .AsNoTracking()
                .ToListAsync();

            // Calculate automatic statistics
            int autoPrograms = rohingyaPrograms.Count;
            int autoEvents = rohingyaEvents.Count;
            int autoBeneficiaries = rohingyaEvents.Sum(e => e.BeneficiaryCount);
            int autoCamps = rohingyaEvents.Where(e => !string.IsNullOrEmpty(e.Thana))
                .Select(e => e.Thana).Distinct().Count();
            if (autoCamps == 0)
                autoCamps = rohingyaEvents.Where(e => !string.IsNullOrEmpty(e.District))
                    .Select(e => e.District).Distinct().Count();

            // Use manual values if provided in settings, otherwise use automatic
            var stats = new
            {
                TotalPrograms = siteSettings?.RohingyaActivePrograms ?? autoPrograms,
                TotalEvents = siteSettings?.RohingyaEventsConducted ?? autoEvents,
                TotalBeneficiaries = siteSettings?.RohingyaTotalBeneficiaries ?? autoBeneficiaries,
                Camps = siteSettings?.RohingyaCampsReached ?? autoCamps,
                Districts = rohingyaEvents.Where(e => !string.IsNullOrEmpty(e.District))
                    .Select(e => e.District).Distinct().Count()
            };

            ViewBag.RohingyaPrograms = rohingyaPrograms;
            ViewBag.RohingyaEvents = rohingyaEvents;
            ViewBag.Stats = stats;
            ViewBag.Appeals = rohingyaAppeals;

            return View();
        }

        // Appeals Page
        public async Task<IActionResult> Appeals()
        {
            var appeals = await _context.Appeals
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(appeals);
        }

        // Health Page
        public async Task<IActionResult> Health()
        {
            var healthSectors = await _context.Sectors
                .Include(s => s.SDG)
                .Where(s => s.IsActive && s.Name.Contains("Health"))
                .ToListAsync();

            return View(healthSectors);
        }

        // Education Page
        public async Task<IActionResult> Education()
        {
            var educationSectors = await _context.Sectors
                .Include(s => s.SDG)
                .Where(s => s.IsActive && s.Name.Contains("Education"))
                .ToListAsync();

            return View(educationSectors);
        }

        // Water & Sanitation Page
        [Route("ourwork/water-sanitation")]
        public async Task<IActionResult> WaterSanitation()
        {
            var waterSectors = await _context.Sectors
                .Include(s => s.SDG)
                .Where(s => s.IsActive && (s.Name.Contains("Water") || s.Name.Contains("Sanitation")))
                .ToListAsync();

            return View(waterSectors);
        }

        // Livelihood Page
        public async Task<IActionResult> Livelihood()
        {
            var livelihoodSectors = await _context.Sectors
                .Include(s => s.SDG)
                .Where(s => s.IsActive && s.Name.Contains("Livelihood"))
                .ToListAsync();

            return View(livelihoodSectors);
        }
    }
}
