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
    public class AnnualReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<AnnualReportsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public AnnualReportsController(
            ApplicationDbContext context, 
            IWebHostEnvironment environment,
            ICloudinaryService cloudinaryService,
            ILogger<AnnualReportsController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _environment = environment;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        // GET: Admin/AnnualReports
        public async Task<IActionResult> Index()
        {
            var reports = await _context.AnnualReports
                .OrderByDescending(r => r.Year)
                .ToListAsync();

            return View(reports);
        }

        // GET: Admin/AnnualReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AnnualReports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnualReport report, IFormFile? pdfFile, IFormFile? coverImageFile)
        {
            try
            {
                // Validate PDF file is uploaded
                if (pdfFile == null || pdfFile.Length == 0)
                {
                    ModelState.AddModelError("pdfFile", "PDF document is required.");
                }

                if (ModelState.IsValid && pdfFile != null)
                {
                    // Upload PDF file to Cloudinary
                    if (pdfFile.Length > 0)
                    {
                        _logger.LogInformation("Uploading PDF to Cloudinary: {FileName}", pdfFile.FileName);
                        
                        var uploadedPdfUrl = await _cloudinaryService.UploadFileAsync(pdfFile, "annualreports/files");
                        
                        if (string.IsNullOrEmpty(uploadedPdfUrl))
                        {
                            _logger.LogError("Failed to upload PDF to Cloudinary");
                            ModelState.AddModelError("pdfFile", "Failed to upload PDF file. Please try again.");
                            ViewBag.ErrorMessage = "Failed to upload PDF to Cloudinary. Please check your internet connection and try again.";
                            return View(report);
                        }
                        
                        report.FileUrl = uploadedPdfUrl;
                        _logger.LogInformation("PDF uploaded successfully: {Url}", uploadedPdfUrl);
                    }

                    // Upload cover image to Cloudinary
                    if (coverImageFile != null && coverImageFile.Length > 0)
                    {
                        _logger.LogInformation("Uploading cover image to Cloudinary: {FileName}", coverImageFile.FileName);
                        
                        var uploadedImageUrl = await _cloudinaryService.UploadImageAsync(coverImageFile, "annualreports/covers");
                        
                        if (string.IsNullOrEmpty(uploadedImageUrl))
                        {
                            _logger.LogError("Failed to upload cover image to Cloudinary");
                            ModelState.AddModelError("coverImageFile", "Failed to upload cover image. Please try again.");
                            ViewBag.ErrorMessage = "Failed to upload cover image to Cloudinary. Please check your internet connection and try again.";
                            return View(report);
                        }
                        
                        report.CoverImage = uploadedImageUrl;
                        _logger.LogInformation("Cover image uploaded successfully: {Url}", uploadedImageUrl);
                    }

                    report.CreatedAt = DateTime.UtcNow;
                    _context.Add(report);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Annual report created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating annual report");
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }

            return View(report);
        }

        // GET: Admin/AnnualReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.AnnualReports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Admin/AnnualReports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AnnualReport report, IFormFile? pdfFile, IFormFile? coverImageFile)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var existingReport = await _context.AnnualReports.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
                    if (existingReport == null)
                    {
                        return NotFound();
                    }

                    // Upload PDF file to Cloudinary
                    if (pdfFile != null && pdfFile.Length > 0)
                    {
                        _logger.LogInformation("Uploading new PDF to Cloudinary for annual report {Id}", id);
                        
                        var uploadedPdfUrl = await _cloudinaryService.UploadFileAsync(pdfFile, "annualreports/files");
                        
                        if (string.IsNullOrEmpty(uploadedPdfUrl))
                        {
                            _logger.LogError("Failed to upload PDF to Cloudinary for annual report {Id}", id);
                            ModelState.AddModelError("pdfFile", "Failed to upload PDF file. Please try again.");
                            ViewBag.ErrorMessage = "Failed to upload PDF to Cloudinary. Please check your internet connection and try again.";
                            report.FileUrl = existingReport.FileUrl;
                            return View(report);
                        }
                        
                        report.FileUrl = uploadedPdfUrl;
                        _logger.LogInformation("PDF uploaded successfully: {Url}", uploadedPdfUrl);
                    }
                    else
                    {
                        report.FileUrl = existingReport.FileUrl;
                    }

                    // Upload cover image to Cloudinary
                    if (coverImageFile != null && coverImageFile.Length > 0)
                    {
                        _logger.LogInformation("Uploading new cover image to Cloudinary for annual report {Id}", id);
                        
                        var uploadedImageUrl = await _cloudinaryService.UploadImageAsync(coverImageFile, "annualreports/covers");
                        
                        if (string.IsNullOrEmpty(uploadedImageUrl))
                        {
                            _logger.LogError("Failed to upload cover image to Cloudinary for annual report {Id}", id);
                            ModelState.AddModelError("coverImageFile", "Failed to upload cover image. Please try again.");
                            ViewBag.ErrorMessage = "Failed to upload cover image to Cloudinary. Please check your internet connection and try again.";
                            report.CoverImage = existingReport.CoverImage;
                            return View(report);
                        }
                        
                        report.CoverImage = uploadedImageUrl;
                        _logger.LogInformation("Cover image uploaded successfully: {Url}", uploadedImageUrl);
                    }
                    else
                    {
                        report.CoverImage = existingReport.CoverImage;
                    }

                    report.CreatedAt = existingReport.CreatedAt;
                    
                    _context.Update(report);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Annual report updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnualReportExists(report.Id))
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
                _logger.LogError(ex, "Error updating annual report {Id}", id);
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }

            return View(report);
        }

        // GET: Admin/AnnualReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.AnnualReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Admin/AnnualReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.AnnualReports.FindAsync(id);
            if (report != null)
            {
                // Note: We're not deleting from Cloudinary to preserve files
                // If you want to delete from Cloudinary, add deletion logic here

                _context.AnnualReports.Remove(report);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Annual report deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/AnnualReports/ViewPDF/5
        [AllowAnonymous] // Allow public access to view PDFs
        public async Task<IActionResult> ViewPDF(int id)
        {
            var report = await _context.AnnualReports.FindAsync(id);
            if (report == null || string.IsNullOrEmpty(report.FileUrl))
            {
                return NotFound();
            }

            try
            {
                // If it's a Cloudinary URL, fetch and stream the PDF
                if (report.FileUrl.Contains("cloudinary.com"))
                {
                    using var httpClient = _httpClientFactory.CreateClient();
                    var response = await httpClient.GetAsync(report.FileUrl);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Failed to fetch PDF from Cloudinary. Status: {StatusCode}", response.StatusCode);
                        return NotFound("PDF not found or inaccessible");
                    }

                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    
                    // Force inline display
                    Response.Headers["Content-Disposition"] = "inline";
                    Response.Headers["X-Content-Type-Options"] = "nosniff";
                    
                    return File(bytes, "application/pdf");
                }
                else
                {
                    // Local file
                    var filePath = Path.Combine(_environment.WebRootPath, report.FileUrl.TrimStart('/'));
                    if (!System.IO.File.Exists(filePath))
                    {
                        return NotFound("PDF file not found");
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
                _logger.LogError(ex, "Error serving PDF for annual report {Id}", id);
                return StatusCode(500, "Error loading PDF");
            }
        }

        // GET: Admin/AnnualReports/DownloadPDF/5
        [AllowAnonymous]
        public async Task<IActionResult> DownloadPDF(int id)
        {
            var report = await _context.AnnualReports.FindAsync(id);
            if (report == null || string.IsNullOrEmpty(report.FileUrl))
            {
                return NotFound();
            }

            try
            {
                if (report.FileUrl.Contains("cloudinary.com"))
                {
                    using var httpClient = _httpClientFactory.CreateClient();
                    var response = await httpClient.GetAsync(report.FileUrl);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Failed to download PDF from Cloudinary. Status: {StatusCode}", response.StatusCode);
                        return NotFound("PDF not found");
                    }

                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    var fileName = $"Annual_Report_{report.Year}_{report.Title}.pdf".Replace(" ", "_");

                    return File(bytes, "application/pdf", fileName);
                }
                else
                {
                    var filePath = Path.Combine(_environment.WebRootPath, report.FileUrl.TrimStart('/'));
                    if (!System.IO.File.Exists(filePath))
                    {
                        return NotFound("PDF file not found");
                    }

                    var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    var fileName = $"Annual_Report_{report.Year}_{report.Title}.pdf".Replace(" ", "_");
                    
                    return File(bytes, "application/pdf", fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading PDF for annual report {Id}", id);
                return StatusCode(500, "Error downloading PDF");
            }
        }

        private bool AnnualReportExists(int id)
        {
            return _context.AnnualReports.Any(e => e.Id == id);
        }
    }
}
