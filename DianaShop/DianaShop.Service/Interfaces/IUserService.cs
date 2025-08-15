using DianaShop.Data.Entities;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
using Microsoft.AspNetCore.Http;

namespace DianaShop.Service.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseModel> CreateUserAsync(UserCreateRequest request);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<UserResponseModel>> GetAllUsersAsync();
        Task<DynamicResponse<UserResponseModel>> GetAllUsersAsync(UserQueryRequest queryRequest);
        Task<UserResponseModel?> GetUserByIdAsync(int id);
        Task<UserResponseModel?> UpdateUserAsync(int id, UserUpdateRequest request);
        Task<UserResponseModel?> AddProfileImageAsync(int id, IFormFile imageFile);
        Task<bool> UpdatePasswordAsync(int id, string oldPassword, string newPassword);
        Task<UserResponseModel?> GetUserByUsernameAsync(string username);
        Task<UserResponseModel?> GetUserByEmailAsync(string email);
    }
}