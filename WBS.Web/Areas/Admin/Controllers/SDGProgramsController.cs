using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Attributes;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Services;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SDGProgramsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<SDGProgramsController> _logger;

        public SDGProgramsController(
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            ICloudinaryService cloudinaryService,
            ILogger<SDGProgramsController> logger)
        {
            _context = context;
            _environment = environment;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }

        // GET: Admin/SDGPrograms
        [Permission("SDG Programs", "View")]
        public async Task<IActionResult> Index(bool? filterRohingya)
        {
            var query = _context.SDGPrograms
                .Include(p => p.SDG)
                .Include(p => p.Events)
                .AsQueryable();

            // Filter for Rohingya programs if requested
            if (filterRohingya == true)
            {
                query = query.Where(p => p.IsRohingyaProgram);
                ViewBag.FilterRohingya = true;
                ViewData["Title"] = "Rohingya Programs";
            }

            var programs = await query
                .OrderBy(p => p.SDG!.Number)
                .ThenBy(p => p.DisplayOrder)
                .ToListAsync();

            return View(programs);
        }

        // GET: Admin/SDGPrograms/Create
        [Permission("SDG Programs", "Create")]
        public async Task<IActionResult> Create()
        {
            var sdgs = await _context.SDGs
                .Where(s => s.IsActive)
                .OrderBy(s => s.Number)
                .ToListAsync();

            ViewBag.SDGs = new SelectList(sdgs, "Id", "Name");
            return View();
        }

        // POST: Admin/SDGPrograms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("SDG Programs", "Create")]
        public async Task<IActionResult> Create(SDGProgram program, IFormFile? featuredImage)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Handle image upload to Cloudinary
                    if (featuredImage != null && featuredImage.Length > 0)
                    {
                        _logger.LogInformation("Uploading featured image to Cloudinary for new SDG Program");
                        var imageUrl = await _cloudinaryService.UploadImageAsync(featuredImage, "sdg-programs");
                        
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            program.FeaturedImage = imageUrl;
                            _logger.LogInformation("Image uploaded successfully to Cloudinary: {ImageUrl}", imageUrl);
                        }
                        else
                        {
                            _logger.LogWarning("Failed to upload image to Cloudinary, saving without image");
                            TempData["Warning"] = "Program created but image upload failed. Please try uploading the image again by editing the program.";
                        }
                    }

                    program.CreatedAt = DateTime.UtcNow;
                    program.CreatedBy = User.Identity?.Name ?? "Admin";

                    _context.SDGPrograms.Add(program);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "SDG Program created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating SDG Program");
                TempData["Error"] = $"Error: {ex.Message}";
            }

            var sdgs = await _context.SDGs
                .Where(s => s.IsActive)
                .OrderBy(s => s.Number)
                .ToListAsync();

            ViewBag.SDGs = new SelectList(sdgs, "Id", "Name", program.SDGId);
            return View(program);
        }

        // GET: Admin/SDGPrograms/Edit/5
        [Permission("SDG Programs", "Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var program = await _context.SDGPrograms.FindAsync(id);
            if (program == null)
            {
                TempData["Error"] = "Program not found";
                return RedirectToAction(nameof(Index));
            }

            var sdgs = await _context.SDGs
                .Where(s => s.IsActive)
                .OrderBy(s => s.Number)
                .ToListAsync();

            ViewBag.SDGs = new SelectList(sdgs, "Id", "Name", program.SDGId);
            return View(program);
        }

        // POST: Admin/SDGPrograms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("SDG Programs", "Edit")]
        public async Task<IActionResult> Edit(int id, SDGProgram program, IFormFile? featuredImage)
        {
            if (id != program.Id)
            {
                TempData["Error"] = "Invalid program ID";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    // Handle image upload to Cloudinary
                    if (featuredImage != null && featuredImage.Length > 0)
                    {
                        _logger.LogInformation("Uploading new featured image to Cloudinary for SDG Program ID: {ProgramId}", id);
                        
                        // Upload new image to Cloudinary
                        var imageUrl = await _cloudinaryService.UploadImageAsync(featuredImage, "sdg-programs");
                        
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            // Delete old image from Cloudinary if it exists
                            if (!string.IsNullOrEmpty(program.FeaturedImage) && program.FeaturedImage.Contains("cloudinary.com"))
                            {
                                try
                                {
                                    var uri = new Uri(program.FeaturedImage);
                                    var pathParts = uri.AbsolutePath.Split('/');
                                    var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 3));
                                    var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                                    var folder = string.Join("/", pathParts.Skip(pathParts.Length - 3).Take(2));
                                    var fullPublicId = $"{folder}/{publicId}";
                                    
                                    _logger.LogInformation("Attempting to delete old image from Cloudinary: {PublicId}", fullPublicId);
                                    await _cloudinaryService.DeleteImageAsync(fullPublicId);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning(ex, "Failed to delete old image from Cloudinary");
                                }
                            }
                            // Delete old local image if it exists
                            else if (!string.IsNullOrEmpty(program.FeaturedImage) && !program.FeaturedImage.Contains("cloudinary.com"))
                            {
                                var oldImagePath = Path.Combine(_environment.WebRootPath, program.FeaturedImage.TrimStart('/'));
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    try
                                    {
                                        System.IO.File.Delete(oldImagePath);
                                        _logger.LogInformation("Deleted old local image: {Path}", oldImagePath);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogWarning(ex, "Failed to delete old local image");
                                    }
                                }
                            }
                            
                            program.FeaturedImage = imageUrl;
                            _logger.LogInformation("Image updated successfully in Cloudinary: {ImageUrl}", imageUrl);
                        }
                        else
                        {
                            _logger.LogWarning("Failed to upload new image to Cloudinary, keeping old image");
                            TempData["Warning"] = "Program updated but image upload failed. Please try uploading the image again.";
                        }
                    }

                    program.UpdatedAt = DateTime.UtcNow;

                    _context.Update(program);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "SDG Program updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating SDG Program");
                TempData["Error"] = $"Error: {ex.Message}";
            }

            var sdgs = await _context.SDGs
                .Where(s => s.IsActive)
                .OrderBy(s => s.Number)
                .ToListAsync();

            ViewBag.SDGs = new SelectList(sdgs, "Id", "Name", program.SDGId);
            return View(program);
        }

        // POST: Admin/SDGPrograms/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("SDG Programs", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var program = await _context.SDGPrograms
                    .Include(p => p.Events)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (program == null)
                {
                    TempData["Error"] = "Program not found";
                    return RedirectToAction(nameof(Index));
                }

                if (program.Events.Any())
                {
                    TempData["Error"] = $"Cannot delete program. It has {program.Events.Count} event(s) associated with it.";
                    return RedirectToAction(nameof(Index));
                }

                // Delete image from Cloudinary if exists
                if (!string.IsNullOrEmpty(program.FeaturedImage))
                {
                    if (program.FeaturedImage.Contains("cloudinary.com"))
                    {
                        try
                        {
                            var uri = new Uri(program.FeaturedImage);
                            var pathParts = uri.AbsolutePath.Split('/');
                            var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 3));
                            var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                            var folder = string.Join("/", pathParts.Skip(pathParts.Length - 3).Take(2));
                            var fullPublicId = $"{folder}/{publicId}";
                            
                            _logger.LogInformation("Deleting image from Cloudinary: {PublicId}", fullPublicId);
                            await _cloudinaryService.DeleteImageAsync(fullPublicId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to delete image from Cloudinary");
                        }
                    }
                    // Delete local image if exists
                    else
                    {
                        var imagePath = Path.Combine(_environment.WebRootPath, program.FeaturedImage.TrimStart('/'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            try
                            {
                                System.IO.File.Delete(imagePath);
                                _logger.LogInformation("Deleted local image: {Path}", imagePath);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "Failed to delete local image");
                            }
                        }
                    }
                }

                _context.SDGPrograms.Remove(program);
                await _context.SaveChangesAsync();

                TempData["Success"] = "SDG Program deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting SDG Program");
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

