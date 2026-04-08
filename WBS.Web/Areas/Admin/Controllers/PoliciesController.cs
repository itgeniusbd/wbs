using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Services;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PoliciesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<PoliciesController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public PoliciesController(
            ApplicationDbContext context, 
            IWebHostEnvironment environment,
            ICloudinaryService cloudinaryService,
            ILogger<PoliciesController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _environment = environment;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        // GET: Admin/Policies
        public async Task<IActionResult> Index()
        {
            var policies = await _context.Policies
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            return View(policies);
        }

        // GET: Admin/Policies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Policies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Policy policy, IFormFile? pdfFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (pdfFile != null && pdfFile.Length > 0)
                    {
                        _logger.LogInformation("Uploading policy PDF to Cloudinary: {FileName}", pdfFile.FileName);
                        
                        var uploadedFileUrl = await _cloudinaryService.UploadFileAsync(pdfFile, "policies");
                        
                        if (string.IsNullOrEmpty(uploadedFileUrl))
                        {
                            _logger.LogError("Failed to upload policy PDF to Cloudinary");
                            ModelState.AddModelError("pdfFile", "Failed to upload PDF. Please try again.");
                            return View(policy);
                        }
                        
                        policy.PdfUrl = uploadedFileUrl;
                        _logger.LogInformation("Policy PDF uploaded successfully: {Url}", uploadedFileUrl);
                    }

                    policy.CreatedAt = DateTime.UtcNow;
                    _context.Add(policy);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Policy created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating policy");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }

            return View(policy);
        }

        // GET: Admin/Policies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        // POST: Admin/Policies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Policy policy, IFormFile? pdfFile)
        {
            if (id != policy.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var existingPolicy = await _context.Policies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    if (existingPolicy == null)
                    {
                        return NotFound();
                    }

                    if (pdfFile != null && pdfFile.Length > 0)
                    {
                        _logger.LogInformation("Uploading new policy PDF to Cloudinary for policy {Id}", id);
                        
                        var uploadedFileUrl = await _cloudinaryService.UploadFileAsync(pdfFile, "policies");
                        
                        if (string.IsNullOrEmpty(uploadedFileUrl))
                        {
                            _logger.LogError("Failed to upload policy PDF to Cloudinary for policy {Id}", id);
                            ModelState.AddModelError("pdfFile", "Failed to upload PDF. Please try again.");
                            policy.PdfUrl = existingPolicy.PdfUrl;
                            return View(policy);
                        }
                        
                        policy.PdfUrl = uploadedFileUrl;
                        _logger.LogInformation("Policy PDF uploaded successfully: {Url}", uploadedFileUrl);
                    }
                    else
                    {
                        policy.PdfUrl = existingPolicy.PdfUrl;
                    }

                    policy.UpdatedAt = DateTime.UtcNow;
                    policy.CreatedAt = existingPolicy.CreatedAt;
                    
                    _context.Update(policy);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Policy updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PolicyExists(policy.Id))
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
                _logger.LogError(ex, "Error updating policy {Id}", id);
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }

            return View(policy);
        }

        // GET: Admin/Policies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        // POST: Admin/Policies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy != null)
            {
                _context.Policies.Remove(policy);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Policy deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Policies/ViewPDF/5
        [AllowAnonymous]
        public async Task<IActionResult> ViewPDF(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null || string.IsNullOrEmpty(policy.PdfUrl))
            {
                return NotFound();
            }

            try
            {
                if (policy.PdfUrl.Contains("cloudinary.com"))
                {
                    using var httpClient = _httpClientFactory.CreateClient();
                    var response = await httpClient.GetAsync(policy.PdfUrl);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Failed to fetch PDF from Cloudinary. Status: {StatusCode}", response.StatusCode);
                        return NotFound("PDF not found or inaccessible");
                    }

                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    
                    Response.Headers["Content-Disposition"] = "inline";
                    Response.Headers["X-Content-Type-Options"] = "nosniff";
                    
                    return File(bytes, "application/pdf");
                }
                else
                {
                    var filePath = Path.Combine(_environment.WebRootPath, policy.PdfUrl.TrimStart('/'));
                    if (!System.IO.File.Exists(filePath))
                    {
                        return NotFound("PDF file not found");
                    }

                    var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    
                    Response.Headers["Content-Disposition"] = "inline";
                    Response.Headers["X-Content-Type-Options"] = "nosniff";
                    
                    return File(bytes, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error serving PDF for policy {Id}", id);
                return StatusCode(500, "Error loading PDF");
            }
        }

        // GET: Admin/Policies/DownloadPDF/5
        [AllowAnonymous]
        public async Task<IActionResult> DownloadPDF(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null || string.IsNullOrEmpty(policy.PdfUrl))
            {
                return NotFound();
            }

            try
            {
                if (policy.PdfUrl.Contains("cloudinary.com"))
                {
                    using var httpClient = _httpClientFactory.CreateClient();
                    var response = await httpClient.GetAsync(policy.PdfUrl);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Failed to download PDF from Cloudinary. Status: {StatusCode}", response.StatusCode);
                        return NotFound("PDF not found");
                    }

                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    var fileName = $"{policy.Title}.pdf".Replace(" ", "_");

                    return File(bytes, "application/pdf", fileName);
                }
                else
                {
                    var filePath = Path.Combine(_environment.WebRootPath, policy.PdfUrl.TrimStart('/'));
                    if (!System.IO.File.Exists(filePath))
                    {
                        return NotFound("PDF file not found");
                    }

                    var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    var fileName = $"{policy.Title}.pdf".Replace(" ", "_");
                    
                    return File(bytes, "application/pdf", fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading PDF for policy {Id}", id);
                return StatusCode(500, "Error downloading PDF");
            }
        }

        private bool PolicyExists(int id)
        {
            return _context.Policies.Any(e => e.Id == id);
        }
    }
}
