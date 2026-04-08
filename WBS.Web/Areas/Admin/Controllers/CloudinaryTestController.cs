using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WBS.Web.Services;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CloudinaryTestController : Controller
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<CloudinaryTestController> _logger;
        private readonly IConfiguration _configuration;

        public CloudinaryTestController(
            ICloudinaryService cloudinaryService, 
            ILogger<CloudinaryTestController> logger,
            IConfiguration configuration)
        {
            _cloudinaryService = cloudinaryService;
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TestUpload(IFormFile testImage)
        {
            try
            {
                _logger.LogInformation("=== Cloudinary Test Upload Started ===");
                
                // Show configuration (without exposing sensitive data)
                var cloudName = _configuration["Cloudinary:CloudName"];
                var hasApiKey = !string.IsNullOrEmpty(_configuration["Cloudinary:ApiKey"]);
                var hasApiSecret = !string.IsNullOrEmpty(_configuration["Cloudinary:ApiSecret"]);
                
                _logger.LogInformation("CloudName: {CloudName}", cloudName);
                _logger.LogInformation("Has ApiKey: {HasKey}", hasApiKey);
                _logger.LogInformation("Has ApiSecret: {HasSecret}", hasApiSecret);

                if (testImage == null || testImage.Length == 0)
                {
                    ViewBag.Error = "Please select an image file.";
                    return View("Index");
                }

                _logger.LogInformation("File: {FileName}, Size: {Size} bytes, Type: {ContentType}", 
                    testImage.FileName, testImage.Length, testImage.ContentType);

                var result = await _cloudinaryService.UploadImageAsync(testImage, "test");

                if (string.IsNullOrEmpty(result))
                {
                    _logger.LogError("Upload failed - null result");
                    ViewBag.Error = "Upload failed. Check the logs for details.";
                    ViewBag.CloudName = cloudName;
                    ViewBag.HasApiKey = hasApiKey;
                    ViewBag.HasApiSecret = hasApiSecret;
                }
                else
                {
                    _logger.LogInformation("Upload successful: {Url}", result);
                    ViewBag.Success = "Upload successful!";
                    ViewBag.ImageUrl = result;
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Test upload failed with exception");
                ViewBag.Error = $"Exception: {ex.Message}";
                return View("Index");
            }
        }
    }
}
