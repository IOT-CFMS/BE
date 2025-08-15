using Azure.Storage.Blobs;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Services
{
    public class BlobService : IBlobService
    {
        private readonly IConfiguration _config;
        private readonly BlobServiceClient _blobServiceClient;
        private static readonly Dictionary<string, string[]> AllowedFileTypes = new()
        {
            { "general", new[] { "image/jpeg", "image/png", "image/gif", "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/vnd.ms-powerpoint", "application/vnd.openxmlformats-officedocument.presentationml.presentation", "text/plain", "video/mp4", "video/mpeg", "video/quicktime", "audio/mpeg", "audio/wav", "application/zip", "application/x-rar-compressed" } },
            { "images", new[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/tiff", "image/webp", "image/x-icon", "image/svg+xml" } },
            { "videos", new[] { "video/mp4", "video/mpeg", "video/quicktime" } }
        };
        public BlobService(IConfiguration config)
        {
            _config = config;
            _blobServiceClient = new BlobServiceClient(_config["BlobService:ConnectionString"]);
        }
        private string GenerateFileName(string fileName)
        {
            try
            {
                string timestamp = DateTime.UtcNow.ToString("yyyyMMddTHHmmss");
                string nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                string extension = Path.GetExtension(fileName);

                return $"{timestamp}_{nameWithoutExt}{extension}";
            }
            catch (Exception ex)
            {
                return fileName;
            }
        }
        public async Task<string> UploadFileAsync(IFormFile file, string fileTypeCategory = "general")
        {

            if (!AllowedFileTypes.ContainsKey(fileTypeCategory) || !AllowedFileTypes[fileTypeCategory].Contains(file.ContentType))
            {
                throw new Exception($"ContentType is invalid. Allowed types: {string.Join(", ", AllowedFileTypes[fileTypeCategory])}");
            }
            try
            {
                var fileName = GenerateFileName(file.FileName);
                var blobContainerClient = _blobServiceClient.GetBlobContainerClient(fileTypeCategory);
                var blobClient = blobContainerClient.GetBlobClient(fileName);
                try
                {
                    using (var stream = file.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, true);
                    }
                    var fileUrl = blobClient.Uri.AbsoluteUri;
                    return fileUrl;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to upload file");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred");
            }
        }
        public async Task<string> UploadVideoAsync(IFormFile file) => await UploadFileAsync(file, "videos");
        public async Task<string> UploadImageAsync(IFormFile file) => await UploadFileAsync(file, "images");
        public async Task<bool> DeleteFileAsync(string fileName, string fileTypeCategory = "general")
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(fileTypeCategory);
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            var result = await blobClient.DeleteIfExistsAsync();
            return result;
        }
        public async Task<bool> DeleteVideoAsync(string fileName) => await DeleteFileAsync(fileName, "videos");
        public async Task<bool> DeleteImageAsync(string fileName) => await DeleteFileAsync(fileName, "images");
    }
}
