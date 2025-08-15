using AutoMapper;
using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseModel?> AddProductAsync(ProductRequestModel model);
        Task<PaginatedResponse<ProductResponseModel>> GetAllProductsAsync(ProductQueryRequest request);
        Task<ProductResponseModel?> GetProductByIdAsync(int id);
        Task UpdateProductAsync(int id, ProductRequestModel model);
        Task DeleteProductAsync(int id);
    }
}