using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;

namespace WBS.Web.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;
        private readonly string _cloudName;

        public CloudinaryService(IConfiguration configuration, ILogger<CloudinaryService> logger)
        {
            _logger = logger;
            
            _cloudName = configuration["Cloudinary:CloudName"] ?? string.Empty;
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            _logger.LogInformation("Initializing Cloudinary with CloudName: {CloudName}, ApiKey exists: {HasApiKey}, ApiSecret exists: {HasApiSecret}", 
                _cloudName, !string.IsNullOrEmpty(apiKey), !string.IsNullOrEmpty(apiSecret));

            if (string.IsNullOrEmpty(_cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                _logger.LogError("Cloudinary credentials are missing or incomplete");
                throw new Exception("Cloudinary credentials are not configured properly.");
            }

            try
            {
                var account = new CloudinaryDotNet.Account(_cloudName, apiKey, apiSecret);
                _cloudinary = new Cloudinary(account);
                _cloudinary.Api.Timeout = 120000; // Set timeout to 120 seconds
                
                // Force TLS 1.2 or higher
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                
                _logger.LogInformation("Cloudinary initialized successfully with timeout: 120s");
                
                // Log the API URL for diagnostics
                _logger.LogInformation("Cloudinary API URL: https://api.cloudinary.com/v1_1/{CloudName}", _cloudName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Cloudinary account");
                throw;
            }
        }

        public async Task<string?> UploadImageAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file provided for upload");
                return null;
            }

            const int maxRetries = 3;
            int currentRetry = 0;

            while (currentRetry < maxRetries)
            {
                try
                {
                    currentRetry++;
                    _logger.LogInformation("Upload attempt {Attempt}/{MaxRetries} - File: {FileName}, Size: {Size} bytes, Folder: {Folder}", 
                        currentRetry, maxRetries, file.FileName, file.Length, folder);

                    using var stream = file.OpenReadStream();
                    
                    // Generate unique filename
                    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
                    
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(uniqueFileName, stream),
                        Folder = $"wbs/{folder}",
                        Transformation = new Transformation().Quality("auto").FetchFormat("auto"),
                        UseFilename = true,
                        UniqueFilename = true,
                        Overwrite = false
                    };

                    _logger.LogInformation("Calling Cloudinary UploadAsync with params: Folder={Folder}, FileName={FileName}", 
                        uploadParams.Folder, uniqueFileName);
                    
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    _logger.LogInformation("Upload result received - StatusCode: {StatusCode}, Error: {Error}", 
                        uploadResult.StatusCode, uploadResult.Error?.Message ?? "None");

                    if (uploadResult.Error != null)
                    {
                        _logger.LogError("Cloudinary upload error - Message: {ErrorMessage}", uploadResult.Error.Message);
                        
                        // Retry on network or server errors (not on auth errors)
                        if (currentRetry < maxRetries)
                        {
                            var delayMs = currentRetry * 1000; // Exponential backoff: 1s, 2s, 3s
                            _logger.LogWarning("Retrying upload after {Delay}ms...", delayMs);
                            await Task.Delay(delayMs);
                            continue;
                        }
                        
                        return null;
                    }

                    if (uploadResult.SecureUrl == null)
                    {
                        _logger.LogError("Cloudinary upload succeeded but SecureUrl is null");
                        
                        if (currentRetry < maxRetries)
                        {
                            _logger.LogWarning("Retrying upload...");
                            await Task.Delay(currentRetry * 1000);
                            continue;
                        }
                        
                        return null;
                    }

                    _logger.LogInformation("Image uploaded successfully to Cloudinary: {SecureUrl}", uploadResult.SecureUrl);
                    return uploadResult.SecureUrl.ToString();
                }
                catch (HttpRequestException httpEx)
                {
                    _logger.LogError(httpEx, "HTTP error on attempt {Attempt}/{MaxRetries} - {Message}. This could indicate network connectivity issues or firewall blocking.", 
                        currentRetry, maxRetries, httpEx.Message);
                    
                    if (currentRetry < maxRetries)
                    {
                        var delayMs = currentRetry * 2000;
                        _logger.LogWarning("Retrying after HTTP error in {Delay}ms...", delayMs);
                        await Task.Delay(delayMs);
                        continue;
                    }
                }
                catch (TaskCanceledException tcEx)
                {
                    _logger.LogError(tcEx, "Upload timeout on attempt {Attempt}/{MaxRetries}. Cloudinary API may be slow or unreachable.", currentRetry, maxRetries);
                    
                    if (currentRetry < maxRetries)
                    {
                        _logger.LogWarning("Retrying after timeout...");
                        await Task.Delay(currentRetry * 2000);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error on attempt {Attempt}/{MaxRetries} uploading to Cloudinary - File: {FileName}. Exception type: {ExceptionType}", 
                        currentRetry, maxRetries, file.FileName, ex.GetType().Name);
                    
                    if (currentRetry < maxRetries)
                    {
                        var delayMs = currentRetry * 2000;
                        _logger.LogWarning("Retrying after error in {Delay}ms...", delayMs);
                        await Task.Delay(delayMs);
                        continue;
                    }
                }
            }

            _logger.LogError("All {MaxRetries} upload attempts failed for file: {FileName}. Please check: 1) Internet connection, 2) Cloudinary credentials, 3) Firewall settings, 4) Cloudinary service status at https://status.cloudinary.com", 
                maxRetries, file.FileName);
            return null;
        }

        public async Task<string?> UploadFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file provided for upload");
                return null;
            }

            const int maxRetries = 3;
            int currentRetry = 0;

            while (currentRetry < maxRetries)
            {
                try
                {
                    currentRetry++;
                    _logger.LogInformation("File upload attempt {Attempt}/{MaxRetries} - File: {FileName}, Size: {Size} bytes, Type: {ContentType}, Folder: {Folder}", 
                        currentRetry, maxRetries, file.FileName, file.Length, file.ContentType, folder);

                    using var stream = file.OpenReadStream();
                    
                    // Generate unique filename
                    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
                    
                    // Use RawUploadParams for non-image files (PDF, DOC, etc.)
                    var uploadParams = new RawUploadParams
                    {
                        File = new FileDescription(uniqueFileName, stream),
                        Folder = $"wbs/{folder}",
                        UseFilename = true,
                        UniqueFilename = true,
                        Overwrite = false
                    };

                    _logger.LogInformation("Calling Cloudinary UploadAsync (Raw) with params: Folder={Folder}, FileName={FileName}", 
                        uploadParams.Folder, uniqueFileName);
                    
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    _logger.LogInformation("Upload result received - StatusCode: {StatusCode}, Error: {Error}", 
                        uploadResult.StatusCode, uploadResult.Error?.Message ?? "None");

                    if (uploadResult.Error != null)
                    {
                        _logger.LogError("Cloudinary file upload error - Message: {ErrorMessage}", uploadResult.Error.Message);
                        
                        // Retry on network or server errors
                        if (currentRetry < maxRetries)
                        {
                            var delayMs = currentRetry * 1000;
                            _logger.LogWarning("Retrying file upload after {Delay}ms...", delayMs);
                            await Task.Delay(delayMs);
                            continue;
                        }
                        
                        return null;
                    }

                    if (uploadResult.SecureUrl == null)
                    {
                        _logger.LogError("Cloudinary file upload succeeded but SecureUrl is null");
                        
                        if (currentRetry < maxRetries)
                        {
                            _logger.LogWarning("Retrying file upload...");
                            await Task.Delay(currentRetry * 1000);
                            continue;
                        }
                        
                        return null;
                    }

                    _logger.LogInformation("File uploaded successfully to Cloudinary: {SecureUrl}", uploadResult.SecureUrl);
                    return uploadResult.SecureUrl.ToString();
                }
                catch (HttpRequestException httpEx)
                {
                    _logger.LogError(httpEx, "HTTP error on file upload attempt {Attempt}/{MaxRetries} - {Message}", 
                        currentRetry, maxRetries, httpEx.Message);
                    
                    if (currentRetry < maxRetries)
                    {
                        var delayMs = currentRetry * 2000;
                        _logger.LogWarning("Retrying file upload after HTTP error in {Delay}ms...", delayMs);
                        await Task.Delay(delayMs);
                        continue;
                    }
                }
                catch (TaskCanceledException tcEx)
                {
                    _logger.LogError(tcEx, "File upload timeout on attempt {Attempt}/{MaxRetries}", currentRetry, maxRetries);
                    
                    if (currentRetry < maxRetries)
                    {
                        _logger.LogWarning("Retrying after timeout...");
                        await Task.Delay(currentRetry * 2000);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error on file upload attempt {Attempt}/{MaxRetries} - File: {FileName}. Exception: {ExceptionType}", 
                        currentRetry, maxRetries, file.FileName, ex.GetType().Name);
                    
                    if (currentRetry < maxRetries)
                    {
                        var delayMs = currentRetry * 2000;
                        _logger.LogWarning("Retrying after error in {Delay}ms...", delayMs);
                        await Task.Delay(delayMs);
                        continue;
                    }
                }
            }

            _logger.LogError("All {MaxRetries} file upload attempts failed for: {FileName}", maxRetries, file.FileName);
            return null;
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return false;

            try
            {
                _logger.LogInformation("Deleting image from Cloudinary: {PublicId}", publicId);
                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);
                
                bool success = result.Result == "ok";
                _logger.LogInformation("Delete result: {Result}, Success: {Success}", result.Result, success);
                
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image from Cloudinary: {PublicId}", publicId);
                return false;
            }
        }

        public async Task<bool> DeleteFileAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return false;

            try
            {
                _logger.LogInformation("Deleting file from Cloudinary: {PublicId}", publicId);
                var deleteParams = new DeletionParams(publicId)
                {
                    ResourceType = ResourceType.Raw
                };
                var result = await _cloudinary.DestroyAsync(deleteParams);
                
                bool success = result.Result == "ok";
                _logger.LogInformation("File delete result: {Result}, Success: {Success}", result.Result, success);
                
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file from Cloudinary: {PublicId}", publicId);
                return false;
            }
        }
    }
}
