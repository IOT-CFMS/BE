using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel.QueryRequest;
using Google.Apis.Storage.v1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = DianaShop.Data.Entities.Image;

namespace DianaShop.Service.Services
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;
        public ImageService(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<PaginatedResponse<ImageResponseModel>> GetAllAsync(ImageQueryRequest request)
        {
            var images = _unitOfWork.Repository<Image>().GetAll()
                .Where(i => request.IsDeleted == null || i.IsDelete == request.IsDeleted)
                .Where(i => request.ImageUrl == null || i.Base64Image.Contains(request.ImageUrl))
                .ProjectTo<ImageResponseModel>(_mapper.ConfigurationProvider);
            return await PaginatedResponse<ImageResponseModel>.CreateAsync(images, request.PageNumber, request.PageSize);
        }

        public async Task<ImageResponseModel?> GetByIdAsync(int id)
        {

            return await _unitOfWork.Repository<Image>().GetAll()
                        .Where(i => i.Id == id)
                        .ProjectTo<ImageResponseModel>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
        }

        //public async Task<Image?> ConvertToImageAsync(IFormFile file)
        //{
        //    if (!AllowedFileTypes.ContainsKey("images") || !AllowedFileTypes["images"].Contains(file.ContentType))
        //    {
        //        return null;
        //        //throw new Exception($"ContentType is invalid. Allowed types: {string.Join(", ", AllowedFileTypes["images"])}");
        //    }
        //    using var memoryStream = new MemoryStream();
        //    await file.CopyToAsync(memoryStream);
        //    var fileBytes = memoryStream.ToArray();
        //    var base64String = Convert.ToBase64String(fileBytes);
        //    var image = new Image
        //    {
        //        Base64Image = base64String,
        //        ContentType = file.ContentType
        //    };
        //    return image;
        //}
        public async Task<Image?> AddSingleAsync(IFormFile file)
        {
            if (file.Length <= 0) return null;
            var imageUrl = await _blobService.UploadImageAsync(file);
            var image = new Image { Base64Image = imageUrl, ContentType = file.ContentType };
            await _unitOfWork.Repository<Image>().InsertAsync(image);
            await _unitOfWork.CommitAsync();
            return image;
        }

        public async Task<List<ImageResponseModel>> AddAsync(List<IFormFile> files)
        {
            var images = new List<Image>();
            foreach (var file in files)
            {
                var image = await AddSingleAsync(file);
                if (image != null) images.Add(image);
            }
            await _unitOfWork.CommitAsync();
            return _mapper.Map<List<ImageResponseModel>>(images);
        }

        public async Task<Image?> AddSingleAsyncB64(string base64)
        {
            if (base64.IsNullOrEmpty()) return null;

            var image = new Image { Base64Image = base64, ContentType = "jpg" };
            await _unitOfWork.Repository<Image>().InsertAsync(image);
            await _unitOfWork.SaveChangesAsync();
            return image;
        }

        //public async Task<List<ImageResponseModel>> AddAsyncB64(List<string> b64s)
        //{
        //    var images = new List<Image>();
        //    foreach (var b64 in b64s)
        //    {
        //        var image = await AddSingleAsyncB64(b64);
        //        if (image != null) images.Add(image);
        //    }
        //    await _unitOfWork.CommitAsync();
        //    return _mapper.Map<List<ImageResponseModel>>(images);
        //}

        public async Task<ImageResponseModel?> UpdateAsync(int id, IFormFile file)
        {
            var image = await _unitOfWork.Repository<Image>().GetById(id);
            if (image == null) return null;
            await _blobService.DeleteImageAsync(image.Base64Image);
            var imageUrl = await _blobService.UploadImageAsync(file);
            image.Base64Image = imageUrl;
            await _unitOfWork.Repository<Image>().Update(image, id);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<ImageResponseModel>(image);
        }

        public async Task DeleteAsync(int id)
        {
            var image = await _unitOfWork.Repository<Image>().GetById(id);
            if (image == null) return;
            var fileName = Path.GetFileName(new Uri(image.Base64Image).LocalPath);
            await _blobService.DeleteImageAsync(fileName);
            await _unitOfWork.Repository<Image>().HardDelete(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task AddImageToProductAsync(int productId, int imageId)
        {
            var image = await GetByIdAsync(imageId);
            var product = await _unitOfWork.Repository<Product>().GetById(productId);
            var existingProductImage = await _unitOfWork.Repository<ProductImage>().GetAll()
                .FirstOrDefaultAsync(pi => pi.ProductId == productId && pi.ImageId == imageId);
            if (image == null || product == null || existingProductImage != null) return;
            var productImage = new ProductImage { ProductId = productId, ImageId = imageId };
            await _unitOfWork.Repository<ProductImage>().InsertAsync(productImage);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveImageFromProductAsync(int productId, int imageId)
        {
            var productImage = await _unitOfWork.Repository<ProductImage>().GetAll()
                .FirstOrDefaultAsync(pi => pi.ProductId == productId && pi.ImageId == imageId);
            if (productImage == null) return;
            _unitOfWork.Repository<ProductImage>().Remove(productImage);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<ImageResponseModel>> GetImagesByProductIdAsync(int productId)
        {
            return await _unitOfWork.Repository<ProductImage>().GetAll()
                .Where(pi => pi.ProductId == productId)
                .Select(pi => pi.Image)
                .ProjectTo<ImageResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        //public async Task AddImageToPostAsync(int postId, int imageId)
        //{
        //    var image = await GetByIdAsync(imageId);
        //    var post = await _unitOfWork.Repository<Post>().GetById(postId);
        //    var existingPostImage = await _unitOfWork.Repository<PostImage>().GetAll()
        //        .FirstOrDefaultAsync(pi => pi.PostId == postId && pi.ImageId == imageId);
        //    if (image == null || post == null || existingPostImage != null) return;
        //    var postImage = new PostImage { PostId = postId, ImageId = imageId };
        //    await _unitOfWork.Repository<PostImage>().InsertAsync(postImage);
        //    await _unitOfWork.CommitAsync();
        //}

        //public async Task RemoveImageFromPostAsync(int postId, int imageId)
        //{
        //    var postImage = await _unitOfWork.Repository<PostImage>().GetAll()
        //        .FirstOrDefaultAsync(pi => pi.PostId == postId && pi.ImageId == imageId);
        //    if (postImage == null) return;
        //    _unitOfWork.Repository<PostImage>().Remove(postImage);
        //    await _unitOfWork.CommitAsync();
        //}

        //public async Task<List<ImageResponseModel>> GetImagesByPostIdAsync(int postId)
        //{
        //    return await _unitOfWork.Repository<PostImage>().GetAll()
        //        .Where(pi => pi.PostId == postId)
        //        .Select(pi => pi.Image)
        //        .ProjectTo<ImageResponseModel>(_mapper.ConfigurationProvider)
        //        .ToListAsync();
        //}
        public async Task SoftDeleteAsync(int id)
        {
            var image = await _unitOfWork.Repository<Image>().GetById(id);
            if (image == null) return;
            image.IsDelete = true;
            await _unitOfWork.CommitAsync();
        }

        public async Task HardDeleteAsync(int id)
        {
            var image = await _unitOfWork.Repository<Image>().GetById(id);
            if (image == null) return;
            await _unitOfWork.Repository<Image>().HardDelete(id);
            await _unitOfWork.CommitAsync();
        }
       

        public async Task AddProfileImageAsync(int userId, IFormFile file)
        {
            var user = await _unitOfWork.Repository<User>().GetById(userId);
            if (user != null && file.Length > 0)
            {
                var imageUrl = await _blobService.UploadImageAsync(file);
                if (imageUrl != null)
                {
                    if (!string.IsNullOrEmpty(user.Image))
                    {
                        try
                        {
                            var fileName = Path.GetFileName(new Uri(user.Image).LocalPath);
                            await _blobService.DeleteImageAsync(fileName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error deleting old image: {ex.Message}");
                        }
                    }
                    user.Image = imageUrl;
                    await _unitOfWork.CommitAsync();
                }
            }
        }
    }
}
