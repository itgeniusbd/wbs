namespace WBS.Web.Services
{
    public interface ICloudinaryService
    {
        Task<string?> UploadImageAsync(IFormFile file, string folder);
        Task<string?> UploadFileAsync(IFormFile file, string folder);
        Task<bool> DeleteImageAsync(string publicId);
        Task<bool> DeleteFileAsync(string publicId);
    }
}
