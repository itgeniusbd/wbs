using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.ViewModels;
using WBS.Web.Services;

namespace WBS.Web.Controllers
{
    public class AboutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IContentService _contentService;

        public AboutController(ApplicationDbContext context, IContentService contentService)
        {
            _context = context;
            _contentService = contentService;
        }

        // About Us (Organization Info)
        public async Task<IActionResult> AboutUs()
        {
            var siteSettings = await _contentService.GetSiteSettingsAsync();
            return View(siteSettings);
        }

        // Who We Are
        public async Task<IActionResult> WhoWeAre()
        {
            var statistics = await _contentService.GetStatisticsAsync();
            return View(statistics);
        }

        // History
        public async Task<IActionResult> History()
        {
            var history = await _context.Histories
                .Where(h => h.IsActive)
                .OrderByDescending(h => h.CreatedAt)
                .FirstOrDefaultAsync();

            return View(history);
        }

        // Where We Work
        public async Task<IActionResult> WhereWeWork()
        {
            var districts = await _context.Districts
                .Include(d => d.Upazilas)
                .OrderBy(d => d.DisplayOrder)
                .ThenBy(d => d.Name)
                .ToListAsync();

            var viewModel = new WhereWeWorkViewModel
            {
                TotalDistricts = 64,
                CoveredDistricts = districts.Count(d => d.HasWork),
                TotalUpazilas = 495,
                CoveredUpazilas = districts.SelectMany(d => d.Upazilas).Count(u => u.HasWork),
                DistrictWorkInfos = districts.Select(d => new DistrictWorkInfo
                {
                    DistrictId = d.Id,
                    DistrictName = d.Name,
                    DistrictNameBn = d.NameBn ?? d.Name,
                    HasWork = d.HasWork,
                    Latitude = d.Latitude,
                    Longitude = d.Longitude,
                    TotalUpazilas = d.Upazilas.Count,
                    CoveredUpazilas = d.Upazilas.Count(u => u.HasWork),
                    UpazilaNames = d.Upazilas.Where(u => u.HasWork).Select(u => u.NameBn ?? u.Name).ToList(),
                    UpazilaNamesEn = d.Upazilas.Where(u => u.HasWork).Select(u => u.Name).ToList()
                }).Where(d => d.HasWork).ToList()
            };

            return View(viewModel);
        }
        
        // TEST NEW MAP VIEW
        public async Task<IActionResult> WhereWeWorkNew()
        {
            var districts = await _context.Districts
                .Include(d => d.Upazilas)
                .OrderBy(d => d.DisplayOrder)
                .ThenBy(d => d.Name)
                .ToListAsync();

            var viewModel = new WhereWeWorkViewModel
            {
                TotalDistricts = 64,
                CoveredDistricts = districts.Count(d => d.HasWork),
                TotalUpazilas = 495,
                CoveredUpazilas = districts.SelectMany(d => d.Upazilas).Count(u => u.HasWork),
                DistrictWorkInfos = districts.Select(d => new DistrictWorkInfo
                {
                    DistrictId = d.Id,
                    DistrictName = d.Name,
                    DistrictNameBn = d.NameBn ?? d.Name,
                    HasWork = d.HasWork,
                    Latitude = d.Latitude,
                    Longitude = d.Longitude,
                    TotalUpazilas = d.Upazilas.Count,
                    CoveredUpazilas = d.Upazilas.Count(u => u.HasWork),
                    UpazilaNames = d.Upazilas.Where(u => u.HasWork).Select(u => u.NameBn ?? u.Name).ToList(),
                    UpazilaNamesEn = d.Upazilas.Where(u => u.HasWork).Select(u => u.Name).ToList()
                }).Where(d => d.HasWork).ToList()
            };

            return View(viewModel);
        }

        // SDGs
        public async Task<IActionResult> SDGs()
        {
            var aboutSDG = await _context.AboutSDGs
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefaultAsync();

            return View(aboutSDG);
        }

        // Legal Status
        public async Task<IActionResult> LegalStatus()
        {
            var legalStatus = await _context.LegalStatuses
                .Include(l => l.RegistrationInfos.Where(r => r.IsActive).OrderBy(r => r.DisplayOrder))
                .FirstOrDefaultAsync();

            return View(legalStatus);
        }

        // Contact
        public IActionResult Contact()
        {
            return View(new ContactFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var message = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Subject = model.Subject,
                Message = model.Message,
                CreatedAt = DateTime.UtcNow
            };

            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thank you for contacting us. We will get back to you soon!";
            return RedirectToAction("Contact");
        }

        // Partnership
        public async Task<IActionResult> Partnership()
        {
            var partners = await _context.Partners
                .Where(p => p.IsActive)
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            return View(partners);
        }
    }
}
