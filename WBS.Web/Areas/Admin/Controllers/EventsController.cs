using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Services;
using WBS.Web.Attributes;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public EventsController(ApplicationDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        [Permission("Events", "View")]
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Include(e => e.Registrations)
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();
            return View(events);
        }

        [Permission("Events", "Create")]
        public IActionResult Create()
        {
            return View(new Event());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Events", "Create")]
        public async Task<IActionResult> Create(Event eventModel, IFormFile? featuredImage)
        {
            if (!ModelState.IsValid)
                return View(eventModel);

            if (featuredImage != null)
            {
                eventModel.FeaturedImage = await _cloudinaryService.UploadImageAsync(featuredImage, "events");
            }

            eventModel.CreatedAt = DateTime.UtcNow;

            _context.Events.Add(eventModel);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Event created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Permission("Events", "Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var eventModel = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventModel == null)
                return NotFound();

            return View(eventModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Events", "Edit")]
        public async Task<IActionResult> Edit(int id, Event eventModel, IFormFile? featuredImage)
        {
            if (id != eventModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(eventModel);

            var existing = await _context.Events.FindAsync(id);
            if (existing == null)
                return NotFound();

            if (featuredImage != null)
            {
                eventModel.FeaturedImage = await _cloudinaryService.UploadImageAsync(featuredImage, "events");
            }
            else
            {
                eventModel.FeaturedImage = existing.FeaturedImage;
            }

            eventModel.CreatedAt = existing.CreatedAt;

            _context.Entry(existing).CurrentValues.SetValues(eventModel);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Event updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Events", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var eventModel = await _context.Events.FindAsync(id);
            if (eventModel != null)
            {
                _context.Events.Remove(eventModel);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Event deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Permission("Events", "View")]
        public async Task<IActionResult> Participants(int id)
        {
            var eventModel = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventModel == null)
                return NotFound();

            ViewBag.Event = eventModel;
            return View(eventModel.Registrations.OrderByDescending(r => r.RegisteredAt).ToList());
        }
    }
}
