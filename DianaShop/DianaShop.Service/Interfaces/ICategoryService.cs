using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseModel>> GetAllAsync();
        Task<CategoryResponseModel?> GetByIdAsync(int id);
        Task<CategoryResponseModel> CreateAsync(CategoryRequestModel request);
        Task<CategoryResponseModel?> UpdateAsync(int id, CategoryRequestModel request);
        Task SoftDeleteAsync(int id);
        Task HardDeleteAsync(int id);
    }
}
