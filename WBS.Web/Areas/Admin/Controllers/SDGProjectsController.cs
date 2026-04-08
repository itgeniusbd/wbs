using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Services;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SDGProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<SDGProjectsController> _logger;

        public SDGProjectsController(
            ApplicationDbContext context, 
            IWebHostEnvironment environment, 
            ICloudinaryService cloudinaryService,
            ILogger<SDGProjectsController> logger)
        {
            _context = context;
            _environment = environment;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }

        // API endpoint to get programs by SDG
        [HttpGet]
        public async Task<IActionResult> GetProgramsBySDG(int sdgId)
        {
            try
            {
                var programs = await _context.SDGPrograms
                    .Where(p => p.SDGId == sdgId && p.IsActive)
                    .OrderBy(p => p.DisplayOrder)
                    .ThenBy(p => p.Title)
                    .Select(p => new
                    {
                        value = p.Id,
                        text = p.Title
                    })
                    .ToListAsync();

                return Json(programs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading programs for SDG {SDGId}", sdgId);
                return Json(new List<object>());
            }
        }

        // API endpoint to get upazilas by district
        [HttpGet]
        public async Task<IActionResult> GetUpazilasByDistrict(int districtId)
        {
            try
            {
                var upazilas = await _context.Upazilas
                    .Where(u => u.DistrictId == districtId)
                    .OrderBy(u => u.Name)
                    .Select(u => new
                    {
                        value = u.Id,
                        text = u.Name,
                        textBn = u.NameBn
                    })
                    .ToListAsync();

                return Json(upazilas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading upazilas for District {DistrictId}", districtId);
                return Json(new List<object>());
            }
        }

        public async Task<IActionResult> Index(int? sdgId, bool? filterRohingya)
        {
            var query = _context.SDGProjects
                .Include(p => p.SDG)
                .Include(p => p.SDGProgram)
                .Include(p => p.Images)
                .AsQueryable();

            if (sdgId.HasValue)
            {
                query = query.Where(p => p.SDGId == sdgId.Value);
            }

            // Filter for Rohingya events if requested
            if (filterRohingya == true)
            {
                query = query.Where(p => p.IsRohingyaEvent);
                ViewBag.FilterRohingya = true;
                ViewData["Title"] = "Rohingya Events";
            }

            var projects = await query
                .OrderBy(p => p.DisplayOrder)
                .ThenByDescending(p => p.CreatedAt)
                .ToListAsync();

            ViewBag.SDGs = await _context.SDGs.OrderBy(s => s.Number).ToListAsync();
            ViewBag.SelectedSDGId = sdgId;

            return View(projects);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.SDGs = new SelectList(await _context.SDGs.OrderBy(s => s.Number).ToListAsync(), "Id", "Name");
            ViewBag.Districts = new SelectList(await _context.Districts.OrderBy(d => d.Name).ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SDGProject project, IFormFile? featuredImage, List<IFormFile>? projectImages)
        {
            if (ModelState.IsValid)
            {
                // Upload featured image to Cloudinary
                if (featuredImage != null && featuredImage.Length > 0)
                {
                    _logger.LogInformation("Uploading featured image to Cloudinary for new SDG Project");
                    var imageUrl = await _cloudinaryService.UploadImageAsync(featuredImage, "sdg-projects");
                    
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        project.FeaturedImage = imageUrl;
                        _logger.LogInformation("Featured image uploaded successfully to Cloudinary: {ImageUrl}", imageUrl);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to upload featured image to Cloudinary");
                        TempData["Warning"] = "Project created but featured image upload failed. Please try uploading the image again by editing the project.";
                    }
                }

                // Set District and Upazila names from IDs
                if (project.DistrictId.HasValue)
                {
                    var district = await _context.Districts.FindAsync(project.DistrictId.Value);
                    if (district != null)
                    {
                        project.District = district.Name;
                        project.DistrictBn = district.NameBn;
                        
                        // Update District HasWork status
                        if (!district.HasWork)
                        {
                            district.HasWork = true;
                            _context.Update(district);
                        }
                    }
                }

                if (project.UpazilaId.HasValue)
                {
                    var upazila = await _context.Upazilas.FindAsync(project.UpazilaId.Value);
                    if (upazila != null)
                    {
                        project.Thana = upazila.Name;
                        project.ThanaBn = upazila.NameBn;
                        
                        // Update Upazila HasWork status
                        if (!upazila.HasWork)
                        {
                            upazila.HasWork = true;
                            _context.Update(upazila);
                        }
                    }
                }

                _context.Add(project);
                await _context.SaveChangesAsync();

                // Upload additional images to Cloudinary
                if (projectImages != null && projectImages.Any())
                {
                    int order = 1;

                    foreach (var image in projectImages)
                    {
                        if (image.Length > 0)
                        {
                            _logger.LogInformation("Uploading project image {Order} to Cloudinary for Project ID: {ProjectId}", order, project.Id);
                            var imageUrl = await _cloudinaryService.UploadImageAsync(image, "sdg-projects");

                            if (!string.IsNullOrEmpty(imageUrl))
                            {
                                var projectImage = new SDGProjectImage
                                {
                                    SDGProjectId = project.Id,
                                    ImageUrl = imageUrl,
                                    DisplayOrder = order++
                                };

                                _context.SDGProjectImages.Add(projectImage);
                                _logger.LogInformation("Project image uploaded successfully to Cloudinary: {ImageUrl}", imageUrl);
                            }
                            else
                            {
                                _logger.LogWarning("Failed to upload project image {Order} to Cloudinary", order);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "SDG Project created successfully!";
                return RedirectToAction(nameof(Index), new { sdgId = project.SDGId });
            }

            ViewBag.SDGs = new SelectList(await _context.SDGs.OrderBy(s => s.Number).ToListAsync(), "Id", "Name", project.SDGId);
            ViewBag.Districts = new SelectList(await _context.Districts.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", project.DistrictId);
            return View(project);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.SDGProjects
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null) return NotFound();

            ViewBag.SDGs = new SelectList(await _context.SDGs.OrderBy(s => s.Number).ToListAsync(), "Id", "Name", project.SDGId);
            ViewBag.Districts = new SelectList(await _context.Districts.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", project.DistrictId);
            
            // Load programs for the selected SDG
            ViewBag.Programs = new SelectList(
                await _context.SDGPrograms
                    .Where(p => p.SDGId == project.SDGId && p.IsActive)
                    .OrderBy(p => p.DisplayOrder)
                    .ToListAsync(), 
                "Id", 
                "Title", 
                project.SDGProgramId);
            
            // Load upazilas for the selected district
            if (project.DistrictId.HasValue)
            {
                ViewBag.Upazilas = new SelectList(
                    await _context.Upazilas
                        .Where(u => u.DistrictId == project.DistrictId.Value)
                        .OrderBy(u => u.Name)
                        .ToListAsync(), 
                    "Id", 
                    "Name", 
                    project.UpazilaId);
            }
            
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SDGProject project, IFormFile? featuredImage, List<IFormFile>? projectImages)
        {
            if (id != project.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Upload new featured image to Cloudinary
                    if (featuredImage != null && featuredImage.Length > 0)
                    {
                        _logger.LogInformation("Uploading new featured image to Cloudinary for SDG Project ID: {ProjectId}", id);
                        
                        var imageUrl = await _cloudinaryService.UploadImageAsync(featuredImage, "sdg-projects");
                        
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            // Delete old image from Cloudinary if it exists
                            if (!string.IsNullOrEmpty(project.FeaturedImage) && project.FeaturedImage.Contains("cloudinary.com"))
                            {
                                try
                                {
                                    var uri = new Uri(project.FeaturedImage);
                                    var pathParts = uri.AbsolutePath.Split('/');
                                    var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 3));
                                    var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                                    var folder = string.Join("/", pathParts.Skip(pathParts.Length - 3).Take(2));
                                    var fullPublicId = $"{folder}/{publicId}";
                                    
                                    _logger.LogInformation("Attempting to delete old featured image from Cloudinary: {PublicId}", fullPublicId);
                                    await _cloudinaryService.DeleteImageAsync(fullPublicId);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning(ex, "Failed to delete old featured image from Cloudinary");
                                }
                            }
                            // Delete old local image if it exists
                            else if (!string.IsNullOrEmpty(project.FeaturedImage) && !project.FeaturedImage.Contains("cloudinary.com"))
                            {
                                var oldImagePath = Path.Combine(_environment.WebRootPath, project.FeaturedImage.TrimStart('/'));
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    try
                                    {
                                        System.IO.File.Delete(oldImagePath);
                                        _logger.LogInformation("Deleted old local featured image: {Path}", oldImagePath);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogWarning(ex, "Failed to delete old local featured image");
                                    }
                                }
                            }
                            
                            project.FeaturedImage = imageUrl;
                            _logger.LogInformation("Featured image updated successfully in Cloudinary: {ImageUrl}", imageUrl);
                        }
                        else
                        {
                            _logger.LogWarning("Failed to upload new featured image to Cloudinary, keeping old image");
                            TempData["Warning"] = "Project updated but featured image upload failed. Please try uploading the image again.";
                        }
                    }

                    // Set District and Upazila names from IDs
                    if (project.DistrictId.HasValue)
                    {
                        var district = await _context.Districts.FindAsync(project.DistrictId.Value);
                        if (district != null)
                        {
                            project.District = district.Name;
                            project.DistrictBn = district.NameBn;
                            
                            // Update District HasWork status
                            if (!district.HasWork)
                            {
                                district.HasWork = true;
                                _context.Update(district);
                            }
                        }
                    }

                    if (project.UpazilaId.HasValue)
                    {
                        var upazila = await _context.Upazilas.FindAsync(project.UpazilaId.Value);
                        if (upazila != null)
                        {
                            project.Thana = upazila.Name;
                            project.ThanaBn = upazila.NameBn;
                            
                            // Update Upazila HasWork status
                            if (!upazila.HasWork)
                            {
                                upazila.HasWork = true;
                                _context.Update(upazila);
                            }
                        }
                    }

                    project.UpdatedAt = DateTime.UtcNow;
                    _context.Update(project);
                    await _context.SaveChangesAsync();

                    // Upload additional images to Cloudinary
                    if (projectImages != null && projectImages.Any())
                    {
                        var maxOrder = project.Images.Any() ? project.Images.Max(i => i.DisplayOrder) : 0;

                        foreach (var image in projectImages)
                        {
                            if (image.Length > 0)
                            {
                                _logger.LogInformation("Uploading additional project image to Cloudinary for Project ID: {ProjectId}", id);
                                var imageUrl = await _cloudinaryService.UploadImageAsync(image, "sdg-projects");

                                if (!string.IsNullOrEmpty(imageUrl))
                                {
                                    var projectImage = new SDGProjectImage
                                    {
                                        SDGProjectId = project.Id,
                                        ImageUrl = imageUrl,
                                        DisplayOrder = ++maxOrder
                                    };

                                    _context.SDGProjectImages.Add(projectImage);
                                    _logger.LogInformation("Additional project image uploaded successfully to Cloudinary: {ImageUrl}", imageUrl);
                                }
                                else
                                {
                                    _logger.LogWarning("Failed to upload additional project image to Cloudinary");
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                    }

                    TempData["Success"] = "SDG Project updated successfully!";
                    return RedirectToAction(nameof(Index), new { sdgId = project.SDGId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            ViewBag.SDGs = new SelectList(await _context.SDGs.OrderBy(s => s.Number).ToListAsync(), "Id", "Name", project.SDGId);
            ViewBag.Districts = new SelectList(await _context.Districts.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", project.DistrictId);
            ViewBag.Programs = new SelectList(
                await _context.SDGPrograms
                    .Where(p => p.SDGId == project.SDGId && p.IsActive)
                    .OrderBy(p => p.DisplayOrder)
                    .ToListAsync(), 
                "Id", 
                "Title", 
                project.SDGProgramId);
            
            if (project.DistrictId.HasValue)
            {
                ViewBag.Upazilas = new SelectList(
                    await _context.Upazilas
                        .Where(u => u.DistrictId == project.DistrictId.Value)
                        .OrderBy(u => u.Name)
                        .ToListAsync(), 
                    "Id", 
                    "Name", 
                    project.UpazilaId);
            }
            
            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.SDGProjectImages.FindAsync(id);
            if (image != null)
            {
                // Delete image from Cloudinary if exists
                if (!string.IsNullOrEmpty(image.ImageUrl))
                {
                    if (image.ImageUrl.Contains("cloudinary.com"))
                    {
                        try
                        {
                            var uri = new Uri(image.ImageUrl);
                            var pathParts = uri.AbsolutePath.Split('/');
                            var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 3));
                            var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                            var folder = string.Join("/", pathParts.Skip(pathParts.Length - 3).Take(2));
                            var fullPublicId = $"{folder}/{publicId}";
                            
                            _logger.LogInformation("Deleting project image from Cloudinary: {PublicId}", fullPublicId);
                            await _cloudinaryService.DeleteImageAsync(fullPublicId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to delete project image from Cloudinary");
                        }
                    }
                    // Delete local image if exists
                    else
                    {
                        var imagePath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            try
                            {
                                System.IO.File.Delete(imagePath);
                                _logger.LogInformation("Deleted local project image: {Path}", imagePath);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "Failed to delete local project image");
                            }
                        }
                    }
                }

                _context.SDGProjectImages.Remove(image);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.SDGProjects
                .Include(p => p.SDG)
                .Include(p => p.SDGProgram)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null) return NotFound();

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.SDGProjects
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project != null)
            {
                // Delete featured image from Cloudinary if exists
                if (!string.IsNullOrEmpty(project.FeaturedImage))
                {
                    if (project.FeaturedImage.Contains("cloudinary.com"))
                    {
                        try
                        {
                            var uri = new Uri(project.FeaturedImage);
                            var pathParts = uri.AbsolutePath.Split('/');
                            var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 3));
                            var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                            var folder = string.Join("/", pathParts.Skip(pathParts.Length - 3).Take(2));
                            var fullPublicId = $"{folder}/{publicId}";
                            
                            _logger.LogInformation("Deleting featured image from Cloudinary: {PublicId}", fullPublicId);
                            await _cloudinaryService.DeleteImageAsync(fullPublicId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to delete featured image from Cloudinary");
                        }
                    }
                    // Delete local image if exists
                    else
                    {
                        var imagePath = Path.Combine(_environment.WebRootPath, project.FeaturedImage.TrimStart('/'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            try
                            {
                                System.IO.File.Delete(imagePath);
                                _logger.LogInformation("Deleted local featured image: {Path}", imagePath);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "Failed to delete local featured image");
                            }
                        }
                    }
                }

                // Delete all project images from Cloudinary
                foreach (var image in project.Images)
                {
                    if (!string.IsNullOrEmpty(image.ImageUrl))
                    {
                        if (image.ImageUrl.Contains("cloudinary.com"))
                        {
                            try
                            {
                                var uri = new Uri(image.ImageUrl);
                                var pathParts = uri.AbsolutePath.Split('/');
                                var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 3));
                                var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                                var folder = string.Join("/", pathParts.Skip(pathParts.Length - 3).Take(2));
                                var fullPublicId = $"{folder}/{publicId}";
                                
                                _logger.LogInformation("Deleting project image from Cloudinary: {PublicId}", fullPublicId);
                                await _cloudinaryService.DeleteImageAsync(fullPublicId);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "Failed to delete project image from Cloudinary");
                            }
                        }
                        // Delete local image if exists
                        else
                        {
                            var imagePath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                try
                                {
                                    System.IO.File.Delete(imagePath);
                                    _logger.LogInformation("Deleted local project image: {Path}", imagePath);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning(ex, "Failed to delete local project image");
                                }
                            }
                        }
                    }
                }

                _context.SDGProjects.Remove(project);
                await _context.SaveChangesAsync();

                TempData["Success"] = "SDG Project deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.SDGProjects.Any(e => e.Id == id);
        }
    }
}
