using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GymManagmentSystem.BLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long _maxFileSize = 5 * 1024 * 1024;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };

        private readonly ILogger<AttachmentService> _logger;
        private readonly IWebHostEnvironment _env;

        public AttachmentService(IWebHostEnvironment env, ILogger<AttachmentService> logger)
        {
            _env = env;
            _logger = logger;
        }

        // Public implementation used internally
        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead || fileStream.Length == 0)
                return null;

            if (fileStream.Length > _maxFileSize)
                return null;

            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            if (!_allowedExtensions.Contains(extension))
                return null;

            var uploadsFolder = Path.Combine(_env.WebRootPath ?? string.Empty, folderName);
            Directory.CreateDirectory(uploadsFolder);

            var storedFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, storedFileName);

            try
            {
                await using var fs = new FileStream(filePath, FileMode.Create);
                await fileStream.CopyToAsync(fs, ct);
                return storedFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload failed");
                return null;
            }
        }

        // Public delete
        public bool Delete(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;

            var path = Path.Combine(_env.WebRootPath ?? string.Empty, folderName, fileName);

            if (!File.Exists(path)) return false;

            File.Delete(path);
            return true;
        }

        public (Stream stream, string ContentType)? GetFile(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            var path = Path.Combine(_env.WebRootPath ?? string.Empty, folderName, fileName);
            if (!File.Exists(path)) return null;

            var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            var contentType = ext switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };

            return (stream, contentType);
        }

        // Explicit interface implementations delegate to the public methods
        async Task<string?> IAttachmentService.UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct)
            => await UploadAsync(fileStream, fileName, folderName, ct);

        bool IAttachmentService.Delete(string fileName, string folderName)
            => Delete(fileName, folderName);

        (Stream stream, string ContentType)? IAttachmentService.GetFile(string fileName, string folderName)
            => GetFile(fileName, folderName);
    }
}