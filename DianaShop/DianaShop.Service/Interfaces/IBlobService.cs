using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IBlobService
    {
        Task<string> UploadFileAsync(IFormFile file, string fileTypeCategory = "general");
        Task<bool> DeleteFileAsync(string fileName, string fileTypeCategory = "general");
        Task<string> UploadVideoAsync(IFormFile file);
        Task<string> UploadImageAsync(IFormFile file);
        Task<bool> DeleteVideoAsync(string fileName);
        Task<bool> DeleteImageAsync(string fileName);
    }
}
