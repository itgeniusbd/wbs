using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Attributes;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Services;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PublicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<PublicationsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public PublicationsController(
            ApplicationDbContext context, 
            IWebHostEnvironment environment,
            ICloudinaryService cloudinaryService,
            ILogger<PublicationsController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _environment = environment;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        // GET: Admin/Publications
        [Permission("Publications", "View")]
        public async Task<IActionResult> Index()
        {
            var publications = await _context.Publications
                .OrderByDescending(p => p.PublishedDate)
                .ToListAsync();
            return View(publications);
        }

        // GET: Admin/Publications/Create
        [Permission("Publications", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Publications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Publications", "Create")]
        public async Task<IActionResult> Create(Publication publication, IFormFile? coverImage, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Upload Cover Image to Cloudinary
                    if (coverImage != null && coverImage.Length > 0)
                    {
                        _logger.LogInformation("Uploading cover image to Cloudinary: {FileName}", coverImage.FileName);
                        
                        var uploadedImageUrl = await _cloudinaryService.UploadImageAsync(coverImage, "publications/covers");
                        
                        if (string.IsNullOrEmpty(uploadedImageUrl))
                        {
                            _logger.LogError("Failed to upload cover image to Cloudinary");
                            ModelState.AddModelError("coverImage", "Failed to upload cover image. Please try again.");
                            ViewBag.ErrorMessage = "Failed to upload cover image to Cloudinary. Please check your internet connection and try again.";
                            return View(publication);
                        }
                        
                        publication.CoverImage = uploadedImageUrl;
                        _logger.LogInformation("Cover image uploaded successfully: {Url}", uploadedImageUrl);
                    }

                    // Upload File (PDF, DOC, etc.) to Cloudinary
                    if (file != null && file.Length > 0)
                    {
                        _logger.LogInformation("Uploading publication file to Cloudinary: {FileName}", file.FileName);
                        
                        var uploadedFileUrl = await _cloudinaryService.UploadFileAsync(file, "publications/files");
                        
                        if (string.IsNullOrEmpty(uploadedFileUrl))
                        {
                            _logger.LogError("Failed to upload publication file to Cloudinary");
                            ModelState.AddModelError("file", "Failed to upload file. Please try again.");
                            ViewBag.ErrorMessage = "Failed to upload publication file to Cloudinary. Please check your internet connection and try again.";
                            return View(publication);
                        }
                        
                        publication.FileUrl = uploadedFileUrl;
                        _logger.LogInformation("Publication file uploaded successfully: {Url}", uploadedFileUrl);
                    }

                    publication.PublishedDate = DateTime.UtcNow;

                    _context.Add(publication);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Publication created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating publication");
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }
            
            return View(publication);
        }

        // GET: Admin/Publications/Edit/5
        [Permission("Publications", "Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications.FindAsync(id);
            if (publication == null)
            {
                return NotFound();
            }
            return View(publication);
        }

        // POST: Admin/Publications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Publications", "Edit")]
        public async Task<IActionResult> Edit(int id, Publication publication, IFormFile? coverImage, IFormFile? file)
        {
            if (id != publication.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var existingPublication = await _context.Publications.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    if (existingPublication == null)
                    {
                        return NotFound();
                    }

                    // Upload Cover Image to Cloudinary
                    if (coverImage != null && coverImage.Length > 0)
                    {
                        _logger.LogInformation("Uploading new cover image to Cloudinary for publication {Id}", id);
                        
                        var uploadedImageUrl = await _cloudinaryService.UploadImageAsync(coverImage, "publications/covers");
                        
                        if (string.IsNullOrEmpty(uploadedImageUrl))
                        {
                            _logger.LogError("Failed to upload cover image to Cloudinary for publication {Id}", id);
                            ModelState.AddModelError("coverImage", "Failed to upload cover image. Please try again.");
                            ViewBag.ErrorMessage = "Failed to upload cover image to Cloudinary. Please check your internet connection and try again.";
                            publication.CoverImage = existingPublication.CoverImage;
                            return View(publication);
                        }
                        
                        publication.CoverImage = uploadedImageUrl;
                        _logger.LogInformation("Cover image uploaded successfully: {Url}", uploadedImageUrl);
                    }
                    else
                    {
                        publication.CoverImage = existingPublication.CoverImage;
                    }

                    // Upload File to Cloudinary
                    if (file != null && file.Length > 0)
                    {
                        _logger.LogInformation("Uploading new publication file to Cloudinary for publication {Id}", id);
                        
                        var uploadedFileUrl = await _cloudinaryService.UploadFileAsync(file, "publications/files");
                        
                        if (string.IsNullOrEmpty(uploadedFileUrl))
                        {
                            _logger.LogError("Failed to upload publication file to Cloudinary for publication {Id}", id);
                            ModelState.AddModelError("file", "Failed to upload file. Please try again.");
                            ViewBag.ErrorMessage = "Failed to upload publication file to Cloudinary. Please check your internet connection and try again.";
                            publication.FileUrl = existingPublication.FileUrl;
                            return View(publication);
                        }
                        
                        publication.FileUrl = uploadedFileUrl;
                        _logger.LogInformation("Publication file uploaded successfully: {Url}", uploadedFileUrl);
                    }
                    else
                    {
                        publication.FileUrl = existingPublication.FileUrl;
                    }

                    _context.Update(publication);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Publication updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicationExists(publication.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating publication {Id}", id);
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }
            
            return View(publication);
        }

        // GET: Admin/Publications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publication = await _context.Publications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publication == null)
            {
                return NotFound();
            }

            return View(publication);
        }

        // POST: Admin/Publications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Permission("Publications", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publication = await _context.Publications.FindAsync(id);
            if (publication != null)
            {
                // Note: We're not deleting from Cloudinary to preserve files
                // If you want to delete from Cloudinary, uncomment the code below
                
                /*
                // Delete cover image from Cloudinary if it's a Cloudinary URL
                if (!string.IsNullOrEmpty(publication.CoverImage) && publication.CoverImage.Contains("cloudinary.com"))
                {
                    // Extract public_id from URL and delete
                    _logger.LogInformation("Deleting cover image from Cloudinary for publication {Id}", id);
                }
                
                // Delete file from Cloudinary if it's a Cloudinary URL
                if (!string.IsNullOrEmpty(publication.FileUrl) && publication.FileUrl.Contains("cloudinary.com"))
                {
                    // Extract public_id from URL and delete
                    _logger.LogInformation("Deleting file from Cloudinary for publication {Id}", id);
                }
                */

                _context.Publications.Remove(publication);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Publication deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Publications/ViewFile/5
        [AllowAnonymous]
        public async Task<IActionResult> ViewFile(int id)
        {
            var publication = await _context.Publications.FindAsync(id);
            if (publication == null || string.IsNullOrEmpty(publication.FileUrl))
            {
                return NotFound();
            }

            try
            {
                if (publication.FileUrl.Contains("cloudinary.com"))
                {
                    using var httpClient = _httpClientFactory.CreateClient();
                    var response = await httpClient.GetAsync(publication.FileUrl);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Failed to fetch file from Cloudinary. Status: {StatusCode}", response.StatusCode);
                        return NotFound("File not found or inaccessible");
                    }

                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    
                    // Force inline display
                    Response.Headers["Content-Disposition"] = "inline";
                    Response.Headers["X-Content-Type-Options"] = "nosniff";
                    
                    return File(bytes, "application/pdf");
                }
                else
                {
                    var filePath = Path.Combine(_environment.WebRootPath, publication.FileUrl.TrimStart('/'));
                    if (!System.IO.File.Exists(filePath))
                    {
                        return NotFound("File not found");
                    }

                    var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    
                    // Force inline display
                    Response.Headers["Content-Disposition"] = "inline";
                    Response.Headers["X-Content-Type-Options"] = "nosniff";
                    
                    return File(bytes, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error serving file for publication {Id}", id);
                return StatusCode(500, "Error loading file");
            }
        }

        // GET: Admin/Publications/DownloadFile/5
        [AllowAnonymous]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var publication = await _context.Publications.FindAsync(id);
            if (publication == null || string.IsNullOrEmpty(publication.FileUrl))
            {
                return NotFound();
            }

            try
            {
                if (publication.FileUrl.Contains("cloudinary.com"))
                {
                    using var httpClient = _httpClientFactory.CreateClient();
                    var response = await httpClient.GetAsync(publication.FileUrl);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Failed to download file from Cloudinary. Status: {StatusCode}", response.StatusCode);
                        return NotFound("File not found");
                    }

                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    var fileName = $"{publication.Title}.pdf".Replace(" ", "_");

                    return File(bytes, "application/pdf", fileName);
                }
                else
                {
                    var filePath = Path.Combine(_environment.WebRootPath, publication.FileUrl.TrimStart('/'));
                    if (!System.IO.File.Exists(filePath))
                    {
                        return NotFound("File not found");
                    }

                    var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    var fileName = $"{publication.Title}.pdf".Replace(" ", "_");
                    
                    return File(bytes, "application/pdf", fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file for publication {Id}", id);
                return StatusCode(500, "Error downloading file");
            }
        }

        private bool PublicationExists(int id)
        {
            return _context.Publications.Any(e => e.Id == id);
        }
    }
}
