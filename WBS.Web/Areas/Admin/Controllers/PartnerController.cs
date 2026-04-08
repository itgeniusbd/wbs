using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Attributes;
using WBS.Web.Services;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PartnerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<PartnerController> _logger;

        public PartnerController(
            ApplicationDbContext context, 
            IWebHostEnvironment environment,
            ICloudinaryService cloudinaryService,
            ILogger<PartnerController> logger)
        {
            _context = context;
            _environment = environment;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }

        // GET: Admin/Partner
        [Permission("Partners & Sponsors", "View")]
        public async Task<IActionResult> Index()
        {
            var partners = await _context.Partners
                .OrderBy(p => p.DisplayOrder)
                .ThenBy(p => p.Name)
                .ToListAsync();

            return View(partners);
        }

        // GET: Admin/Partner/Create
        [Permission("Partners & Sponsors", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Partner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Partners & Sponsors", "Create")]
        public async Task<IActionResult> Create(Partner partner, IFormFile? logoFile)
        {
            // Remove NotMapped fields from ModelState validation
            ModelState.Remove("Description");
            ModelState.Remove("DescriptionBn");
            ModelState.Remove("PartnerType");
            ModelState.Remove("Email");
            ModelState.Remove("Phone");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("UpdatedAt");

            if (ModelState.IsValid)
            {
                // Upload logo to Cloudinary if provided
                if (logoFile != null && logoFile.Length > 0)
                {
                    _logger.LogInformation("Uploading partner logo to Cloudinary");
                    var imageUrl = await _cloudinaryService.UploadImageAsync(logoFile, "partners");
                    
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        partner.Logo = imageUrl;
                        _logger.LogInformation("Partner logo uploaded successfully to Cloudinary: {ImageUrl}", imageUrl);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to upload partner logo to Cloudinary");
                        TempData["Warning"] = "Partner added but logo upload failed. Please try uploading the logo again by editing the partner.";
                    }
                }

                _context.Partners.Add(partner);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Partner added successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(partner);
        }

        // GET: Admin/Partner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var partner = await _context.Partners.FindAsync(id);
            if (partner == null)
                return NotFound();

            return View(partner);
        }

        // POST: Admin/Partner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Partner partner, IFormFile? logoFile)
        {
            if (id != partner.Id)
                return NotFound();

            // Remove NotMapped fields from ModelState validation
            ModelState.Remove("Description");
            ModelState.Remove("DescriptionBn");
            ModelState.Remove("PartnerType");
            ModelState.Remove("Email");
            ModelState.Remove("Phone");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("UpdatedAt");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPartner = await _context.Partners.FindAsync(id);
                    if (existingPartner == null)
                        return NotFound();

                    // Upload new logo to Cloudinary if provided
                    if (logoFile != null && logoFile.Length > 0)
                    {
                        _logger.LogInformation("Uploading new partner logo to Cloudinary for Partner ID: {PartnerId}", id);
                        
                        var imageUrl = await _cloudinaryService.UploadImageAsync(logoFile, "partners");
                        
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            // Delete old logo from Cloudinary if it exists
                            if (!string.IsNullOrEmpty(existingPartner.Logo) && existingPartner.Logo.Contains("cloudinary.com"))
                            {
                                try
                                {
                                    var uri = new Uri(existingPartner.Logo);
                                    var pathParts = uri.AbsolutePath.Split('/');
                                    var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 2));
                                    var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                                    var folder = pathParts[pathParts.Length - 2];
                                    var fullPublicId = $"{folder}/{publicId}";
                                    
                                    _logger.LogInformation("Attempting to delete old partner logo from Cloudinary: {PublicId}", fullPublicId);
                                    await _cloudinaryService.DeleteImageAsync(fullPublicId);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning(ex, "Failed to delete old partner logo from Cloudinary");
                                }
                            }
                            // Delete old local logo if it exists
                            else if (!string.IsNullOrEmpty(existingPartner.Logo) && !existingPartner.Logo.Contains("cloudinary.com"))
                            {
                                var oldPath = Path.Combine(_environment.WebRootPath, existingPartner.Logo.TrimStart('/'));
                                if (System.IO.File.Exists(oldPath))
                                {
                                    try
                                    {
                                        System.IO.File.Delete(oldPath);
                                        _logger.LogInformation("Deleted old local partner logo: {Path}", oldPath);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogWarning(ex, "Failed to delete old local partner logo");
                                    }
                                }
                            }
                            
                            existingPartner.Logo = imageUrl;
                            _logger.LogInformation("Partner logo updated successfully in Cloudinary: {ImageUrl}", imageUrl);
                        }
                        else
                        {
                            _logger.LogWarning("Failed to upload new partner logo to Cloudinary, keeping old logo");
                            TempData["Warning"] = "Partner updated but logo upload failed. Please try uploading the logo again.";
                        }
                    }

                    existingPartner.Name = partner.Name;
                    existingPartner.NameBn = partner.NameBn;
                    existingPartner.Website = partner.Website;
                    existingPartner.FacebookUrl = partner.FacebookUrl;
                    existingPartner.TwitterUrl = partner.TwitterUrl;
                    existingPartner.LinkedInUrl = partner.LinkedInUrl;
                    existingPartner.InstagramUrl = partner.InstagramUrl;
                    existingPartner.YouTubeUrl = partner.YouTubeUrl;
                    existingPartner.DisplayOrder = partner.DisplayOrder;
                    existingPartner.IsActive = partner.IsActive;

                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Partner updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartnerExists(partner.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(partner);
        }

        // POST: Admin/Partner/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var partner = await _context.Partners.FindAsync(id);
            if (partner != null)
            {
                // Delete logo from Cloudinary if exists
                if (!string.IsNullOrEmpty(partner.Logo))
                {
                    if (partner.Logo.Contains("cloudinary.com"))
                    {
                        try
                        {
                            var uri = new Uri(partner.Logo);
                            var pathParts = uri.AbsolutePath.Split('/');
                            var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 2));
                            var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                            var folder = pathParts[pathParts.Length - 2];
                            var fullPublicId = $"{folder}/{publicId}";
                            
                            _logger.LogInformation("Deleting partner logo from Cloudinary: {PublicId}", fullPublicId);
                            await _cloudinaryService.DeleteImageAsync(fullPublicId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to delete partner logo from Cloudinary");
                        }
                    }
                    // Delete local logo if exists
                    else
                    {
                        var filePath = Path.Combine(_environment.WebRootPath, partner.Logo.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            try
                            {
                                System.IO.File.Delete(filePath);
                                _logger.LogInformation("Deleted local partner logo: {Path}", filePath);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "Failed to delete local partner logo");
                            }
                        }
                    }
                }

                _context.Partners.Remove(partner);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Partner deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PartnerExists(int id)
        {
            return _context.Partners.Any(e => e.Id == id);
        }
    }
}

