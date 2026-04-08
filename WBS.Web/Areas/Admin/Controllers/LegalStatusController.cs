using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Attributes;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class LegalStatusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public LegalStatusController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/LegalStatus
        [Permission("Legal Status", "View")]
        public async Task<IActionResult> Index()
        {
            var legalStatus = await _context.LegalStatuses
                .Include(l => l.RegistrationInfos.OrderBy(r => r.DisplayOrder))
                .FirstOrDefaultAsync();

            if (legalStatus == null)
            {
                // Create default if not exists
                legalStatus = new LegalStatus();
                _context.LegalStatuses.Add(legalStatus);
                await _context.SaveChangesAsync();
            }

            return View(legalStatus);
        }

        // POST: Admin/LegalStatus/UpdateCertificate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Legal Status", "Edit")]
        public async Task<IActionResult> UpdateCertificate(IFormFile? certificateImage, IFormFile? certificateImageBn)
        {
            var legalStatus = await _context.LegalStatuses.FirstOrDefaultAsync();
            
            if (legalStatus == null)
            {
                legalStatus = new LegalStatus();
                _context.LegalStatuses.Add(legalStatus);
            }

            // Upload English Certificate Image
            if (certificateImage != null && certificateImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "legal-status");
                Directory.CreateDirectory(uploadsFolder);

                // Delete old image
                if (!string.IsNullOrEmpty(legalStatus.CertificateImage))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath, legalStatus.CertificateImage.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var uniqueFileName = $"certificate_en_{Guid.NewGuid()}{Path.GetExtension(certificateImage.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await certificateImage.CopyToAsync(fileStream);
                }

                legalStatus.CertificateImage = $"/uploads/legal-status/{uniqueFileName}";
            }

            // Upload Bangla Certificate Image
            if (certificateImageBn != null && certificateImageBn.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "legal-status");
                Directory.CreateDirectory(uploadsFolder);

                // Delete old image
                if (!string.IsNullOrEmpty(legalStatus.CertificateImageBn))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath, legalStatus.CertificateImageBn.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var uniqueFileName = $"certificate_bn_{Guid.NewGuid()}{Path.GetExtension(certificateImageBn.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await certificateImageBn.CopyToAsync(fileStream);
                }

                legalStatus.CertificateImageBn = $"/uploads/legal-status/{uniqueFileName}";
            }

            legalStatus.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Certificate images updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/LegalStatus/CreateRegistration
        [Permission("Legal Status", "Create")]
        public IActionResult CreateRegistration()
        {
            return View();
        }

        // POST: Admin/LegalStatus/CreateRegistration
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Legal Status", "Create")]
        public async Task<IActionResult> CreateRegistration(RegistrationInfo registrationInfo)
        {
            var legalStatus = await _context.LegalStatuses.FirstOrDefaultAsync();
            if (legalStatus == null)
            {
                legalStatus = new LegalStatus();
                _context.LegalStatuses.Add(legalStatus);
                await _context.SaveChangesAsync();
            }

            registrationInfo.LegalStatusId = legalStatus.Id;
            
            if (ModelState.IsValid)
            {
                _context.RegistrationInfos.Add(registrationInfo);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Registration information added successfully!";
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    TempData["Error"] = error.ErrorMessage;
                }
            }

            return View(registrationInfo);
        }

        // GET: Admin/LegalStatus/EditRegistration/5
        public async Task<IActionResult> EditRegistration(int? id)
        {
            if (id == null)
                return NotFound();

            var registrationInfo = await _context.RegistrationInfos.FindAsync(id);
            if (registrationInfo == null)
                return NotFound();

            return View(registrationInfo);
        }

        // POST: Admin/LegalStatus/EditRegistration/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRegistration(int id, RegistrationInfo registrationInfo)
        {
            if (id != registrationInfo.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registrationInfo);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Registration information updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationInfoExists(registrationInfo.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(registrationInfo);
        }

        // POST: Admin/LegalStatus/DeleteRegistration/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var registrationInfo = await _context.RegistrationInfos.FindAsync(id);
            if (registrationInfo != null)
            {
                _context.RegistrationInfos.Remove(registrationInfo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Registration information deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationInfoExists(int id)
        {
            return _context.RegistrationInfos.Any(e => e.Id == id);
        }
    }
}
